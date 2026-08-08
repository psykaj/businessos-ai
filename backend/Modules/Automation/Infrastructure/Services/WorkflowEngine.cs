using System;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.Automation.Application.Interfaces;
using backend.Modules.Automation.Domain.Entities;
using backend.Modules.Automation.Domain.Enums;

namespace backend.Modules.Automation.Infrastructure.Services;

public class WorkflowEngine : IWorkflowEngine
{
    private readonly IWorkflowExecutionLogger _logger;
    private readonly IWorkflowConditionEvaluator _conditionEvaluator;
    private readonly IAIDecisionService _aiDecisionService;
    private readonly IWorkflowApprovalService _approvalService;
    private readonly IWorkflowActionExecutor _actionExecutor;

    public WorkflowEngine(
        IWorkflowExecutionLogger logger,
        IWorkflowConditionEvaluator conditionEvaluator,
        IAIDecisionService aiDecisionService,
        IWorkflowApprovalService approvalService,
        IWorkflowActionExecutor actionExecutor)
    {
        _logger = logger;
        _conditionEvaluator = conditionEvaluator;
        _aiDecisionService = aiDecisionService;
        _approvalService = approvalService;
        _actionExecutor = actionExecutor;
    }

    public async Task ExecuteWorkflowAsync(AiWorkflow workflow, Guid organizationId, object eventData)
    {
        var execution = await _logger.StartExecutionAsync(workflow.Id, organizationId, workflow.TriggerType, eventData);

        try
        {
            var sortedSteps = workflow.Steps.OrderBy(s => s.StepOrder).ToList();

            foreach (var step in sortedSteps)
            {
                var executionStep = await _logger.StartStepAsync(execution.Id, step.Id);

                try
                {
                    // 1. Evaluate Condition Step
                    if (step.StepType == WorkflowStepType.Condition.ToString())
                    {
                        bool conditionMet = _conditionEvaluator.EvaluateCondition(step, eventData);
                        await _logger.UpdateStepStatusAsync(executionStep.Id, WorkflowExecutionStatus.Completed, $"Condition Met: {conditionMet}");
                        
                        if (!conditionMet)
                        {
                            // Workflow stops if condition is not met
                            await _logger.UpdateExecutionStatusAsync(execution.Id, WorkflowExecutionStatus.Completed, "Condition not met. Workflow stopped.");
                            return; 
                        }
                    }
                    // 2. AI Decision Step
                    else if (step.StepType == WorkflowStepType.AIDecision.ToString())
                    {
                        var aiResult = await _aiDecisionService.MakeDecisionAsync(step, organizationId, eventData);
                        await _logger.UpdateStepStatusAsync(executionStep.Id, WorkflowExecutionStatus.Completed, System.Text.Json.JsonSerializer.Serialize(aiResult));
                        
                        if (aiResult.Decision.Equals("Reject", StringComparison.OrdinalIgnoreCase))
                        {
                            await _logger.UpdateExecutionStatusAsync(execution.Id, WorkflowExecutionStatus.Completed, "AI rejected the continuation of this workflow.");
                            return;
                        }
                    }
                    // 3. Action Step
                    else if (step.StepType == WorkflowStepType.Action.ToString())
                    {
                        if (workflow.RequiresApproval || step.Configuration?.Contains("\"RequiresApproval\": true") == true)
                        {
                            await _approvalService.RequestApprovalAsync(step, execution.Id, organizationId);
                            // Execution stops here and resumes after approval
                            return;
                        }

                        bool actionResult = await _actionExecutor.ExecuteActionAsync(step, organizationId, eventData);
                        await _logger.UpdateStepStatusAsync(executionStep.Id, WorkflowExecutionStatus.Completed, $"Action executed. Result: {actionResult}");
                    }
                    // 4. Approval Step (Explicit)
                    else if (step.StepType == WorkflowStepType.Approval.ToString())
                    {
                        await _approvalService.RequestApprovalAsync(step, execution.Id, organizationId);
                        return; // Stop and wait for approval
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message == "integration_required")
                    {
                        await _logger.UpdateStepStatusAsync(executionStep.Id, WorkflowExecutionStatus.Failed, errorMessage: "Integration required but not configured");
                        await _logger.UpdateExecutionStatusAsync(execution.Id, WorkflowExecutionStatus.Failed, "Workflow halted due to missing integration.");
                        return;
                    }
                    
                    await _logger.UpdateStepStatusAsync(executionStep.Id, WorkflowExecutionStatus.Failed, errorMessage: ex.Message);
                    if (step.IsRequired)
                    {
                        throw; // Stop workflow if required step fails
                    }
                }
            }

            await _logger.UpdateExecutionStatusAsync(execution.Id, WorkflowExecutionStatus.Completed);
        }
        catch (Exception ex)
        {
            await _logger.UpdateExecutionStatusAsync(execution.Id, WorkflowExecutionStatus.Failed, ex.Message);
        }
    }
}
