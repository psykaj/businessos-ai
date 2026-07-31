using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Entities;
using backend.Modules.AutomationStudio.ExecutionEngine.Interfaces;
using backend.Modules.AutomationStudio.Conditions.Interfaces;
using backend.Modules.AutomationStudio.Actions.Interfaces;

namespace backend.Modules.AutomationStudio.ExecutionEngine.Services;

public class WorkflowExecutionEngine : IWorkflowExecutionEngine
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConditionService _conditionService;
    private readonly IActionService _actionService;

    public WorkflowExecutionEngine(
        ApplicationDbContext dbContext,
        IConditionService conditionService,
        IActionService actionService)
    {
        _dbContext = dbContext;
        _conditionService = conditionService;
        _actionService = actionService;
    }

    public async Task ExecuteWorkflowAsync(Guid workflowId, string contextData)
    {
        var workflow = await _dbContext.AutomationWorkflows
            .Include(w => w.Conditions)
            .Include(w => w.Actions)
            .FirstOrDefaultAsync(w => w.Id == workflowId && w.IsActive && !w.IsDeleted);

        if (workflow == null)
            return;

        // Create Execution Record
        var execution = new backend.Entities.WorkflowExecution
        {
            OrganizationId = workflow.OrganizationId,
            WorkflowId = workflowId,
            Status = "InProgress",
            StartedAt = DateTime.UtcNow,
            ContextData = contextData
        };
        _dbContext.StudioWorkflowExecutions.Add(execution);
        await _dbContext.SaveChangesAsync();

        bool conditionsPassed = true;

        // Evaluate Conditions (Assuming ALL conditions must pass - simple AND logic for now)
        foreach (var condition in workflow.Conditions.OrderBy(c => c.Order))
        {
            bool passed = false;
            string message = "";
            try
            {
                passed = await _conditionService.EvaluateConditionAsync(condition, contextData);
                message = passed ? "Condition Passed" : "Condition Failed";
            }
            catch (Exception ex)
            {
                message = $"Error evaluating condition: {ex.Message}";
            }

            _dbContext.StudioExecutionLogs.Add(new ExecutionLog
            {
                OrganizationId = workflow.OrganizationId,
                WorkflowExecutionId = execution.Id,
                StepType = "Condition",
                StepId = condition.Id,
                Status = passed ? "Success" : "Failed",
                Message = $"{{\"info\": \"{message}\"}}"
            });

            if (!passed)
            {
                conditionsPassed = false;
                break;
            }
        }

        if (!conditionsPassed)
        {
            await CompleteExecution(execution, "Completed (Conditions Failed)");
            return;
        }

        // Execute Actions sequentially
        foreach (var action in workflow.Actions.OrderBy(a => a.Order))
        {
            bool success = false;
            string message = "";
            try
            {
                success = await _actionService.ExecuteActionAsync(action, contextData);
                message = success ? "Action Executed" : "Action Failed";
            }
            catch (Exception ex)
            {
                message = $"Error executing action: {ex.Message}";
            }

            _dbContext.StudioExecutionLogs.Add(new ExecutionLog
            {
                OrganizationId = workflow.OrganizationId,
                WorkflowExecutionId = execution.Id,
                StepType = "Action",
                StepId = action.Id,
                Status = success ? "Success" : "Failed",
                Message = $"{{\"info\": \"{message}\"}}"
            });

            if (!success)
            {
                await CompleteExecution(execution, "Failed");
                return;
            }
        }

        await CompleteExecution(execution, "Completed");
    }

    private async Task CompleteExecution(backend.Entities.WorkflowExecution execution, string status)
    {
        execution.Status = status;
        execution.CompletedAt = DateTime.UtcNow;
        _dbContext.StudioWorkflowExecutions.Update(execution);
        await _dbContext.SaveChangesAsync();
    }
}
