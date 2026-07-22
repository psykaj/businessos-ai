using System.Diagnostics;
using System.Text.Json;
using backend.Modules.Workflow.Actions;
using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.Entities;
using backend.Modules.Workflow.Interfaces;
using Microsoft.Extensions.Logging;

namespace backend.Modules.Workflow.Executions;

public class WorkflowExecutionEngine : IWorkflowExecutionEngine
{
    private readonly IExecutionRepository _executionRepository;
    private readonly ILogRepository _logRepository;
    private readonly ActionRegistry _actionRegistry;
    private readonly IConditionEvaluator _conditionEvaluator;
    private readonly ILogger<WorkflowExecutionEngine> _logger;

    public WorkflowExecutionEngine(
        IExecutionRepository executionRepository,
        ILogRepository logRepository,
        ActionRegistry actionRegistry,
        IConditionEvaluator conditionEvaluator,
        ILogger<WorkflowExecutionEngine> logger)
    {
        _executionRepository = executionRepository;
        _logRepository = logRepository;
        _actionRegistry = actionRegistry;
        _conditionEvaluator = conditionEvaluator;
        _logger = logger;
    }

    public async Task<WorkflowExecution> ExecuteWorkflowAsync(
        Entities.Workflow workflow,
        Dictionary<string, string> initialContext,
        string executedBy = "System",
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var context = new Dictionary<string, string>(initialContext, StringComparer.OrdinalIgnoreCase);

        var execution = new WorkflowExecution
        {
            WorkflowId = workflow.Id,
            OrganizationId = workflow.OrganizationId,
            StartedAt = DateTime.UtcNow,
            Status = WorkflowExecutionStatus.Running,
            ExecutedBy = executedBy,
            TriggerDataJson = JsonSerializer.Serialize(initialContext),
            ContextDataJson = JsonSerializer.Serialize(context)
        };

        execution = await _executionRepository.AddAsync(execution, cancellationToken);
        _logger.LogInformation("Starting Workflow Execution {ExecutionId} for Workflow {WorkflowId}", execution.Id, workflow.Id);

        try
        {
            // Evaluate global workflow conditions if any
            if (workflow.Conditions != null && workflow.Conditions.Count > 0)
            {
                var globalCondsMet = _conditionEvaluator.EvaluateConditions(workflow.Conditions, context);
                if (!globalCondsMet)
                {
                    execution.Status = WorkflowExecutionStatus.Completed;
                    execution.FinishedAt = DateTime.UtcNow;
                    execution.DurationMs = stopwatch.ElapsedMilliseconds;
                    execution.ErrorMessage = "Workflow execution skipped: Global conditions not met.";
                    await _executionRepository.UpdateAsync(execution, cancellationToken);
                    return execution;
                }
            }

            var sortedActions = workflow.Actions.OrderBy(a => a.ExecutionOrder).ToList();

            foreach (var action in sortedActions)
            {
                // Check action-level conditions
                if (action.Conditions != null && action.Conditions.Count > 0)
                {
                    bool condMet = _conditionEvaluator.EvaluateConditions(action.Conditions, context);
                    if (!condMet)
                    {
                        _logger.LogInformation("Skipping Action {ActionId} ({ActionType}): Action conditions not met.", action.Id, action.ActionType);
                        continue;
                    }
                }

                var stepLog = new WorkflowExecutionLog
                {
                    ExecutionId = execution.Id,
                    WorkflowId = workflow.Id,
                    StepName = action.ActionType.ToString(),
                    StepType = action.ActionType.ToString(),
                    Status = WorkflowExecutionStatus.Running,
                    StartedAt = DateTime.UtcNow,
                    InputJson = action.Configuration
                };

                var handler = _actionRegistry.GetHandler(action.ActionType);
                if (handler == null)
                {
                    stepLog.Status = WorkflowExecutionStatus.Failed;
                    stepLog.FinishedAt = DateTime.UtcNow;
                    stepLog.ErrorMessage = $"No handler registered for action type {action.ActionType}";
                    await _logRepository.AddAsync(stepLog, cancellationToken);
                    continue;
                }

                // Execute with retry logic
                int attempt = 0;
                ActionResult result = new(false, "{}", "Execution failed");

                while (attempt < WorkflowConstants.MaxRetryAttempts)
                {
                    attempt++;
                    try
                    {
                        result = await handler.ExecuteAsync(workflow.OrganizationId, action.Configuration, context, cancellationToken);
                        if (result.Success) break;
                    }
                    catch (Exception ex)
                    {
                        result = new ActionResult(false, "{}", ex.Message);
                    }

                    if (attempt < WorkflowConstants.MaxRetryAttempts)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(200 * attempt), cancellationToken);
                    }
                }

                stepLog.FinishedAt = DateTime.UtcNow;
                stepLog.OutputJson = result.OutputJson;

                if (result.Success)
                {
                    stepLog.Status = WorkflowExecutionStatus.Completed;
                    if (result.OutputContext != null)
                    {
                        foreach (var kvp in result.OutputContext)
                        {
                            context[kvp.Key] = kvp.Value;
                        }
                    }
                }
                else
                {
                    stepLog.Status = WorkflowExecutionStatus.Failed;
                    stepLog.ErrorMessage = result.ErrorMessage;
                }

                await _logRepository.AddAsync(stepLog, cancellationToken);

                if (!result.Success)
                {
                    _logger.LogWarning("Action {ActionType} failed for execution {ExecutionId}: {Error}", action.ActionType, execution.Id, result.ErrorMessage);
                }
            }

            stopwatch.Stop();
            execution.FinishedAt = DateTime.UtcNow;
            execution.DurationMs = stopwatch.ElapsedMilliseconds;
            execution.Status = WorkflowExecutionStatus.Completed;
            execution.ContextDataJson = JsonSerializer.Serialize(context);
            await _executionRepository.UpdateAsync(execution, cancellationToken);

            _logger.LogInformation("Completed Workflow Execution {ExecutionId} in {Duration}ms", execution.Id, execution.DurationMs);
            return execution;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            execution.FinishedAt = DateTime.UtcNow;
            execution.DurationMs = stopwatch.ElapsedMilliseconds;
            execution.Status = WorkflowExecutionStatus.Failed;
            execution.ErrorMessage = ex.Message;
            await _executionRepository.UpdateAsync(execution, cancellationToken);

            _logger.LogError(ex, "Failed Workflow Execution {ExecutionId}", execution.Id);
            return execution;
        }
    }
}
