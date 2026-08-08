using System;
using System.Threading.Tasks;
using backend.Persistence;
using backend.Modules.Automation.Application.Interfaces;
using backend.Modules.Automation.Domain.Entities;
using backend.Modules.Automation.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Automation.Infrastructure.Services;

public class WorkflowExecutionLogger : IWorkflowExecutionLogger
{
    private readonly ApplicationDbContext _context;

    public WorkflowExecutionLogger(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkflowExecution> StartExecutionAsync(Guid workflowId, Guid organizationId, string triggeredBy, object eventData)
    {
        var execution = new WorkflowExecution
        {
            WorkflowId = workflowId,
            OrganizationId = organizationId,
            StartedAt = DateTime.UtcNow,
            Status = WorkflowExecutionStatus.InProgress.ToString(),
            TriggeredBy = triggeredBy,
            ExecutionContext = System.Text.Json.JsonSerializer.Serialize(eventData)
        };

        _context.AiEngineWorkflowExecutions.Add(execution);
        await _context.SaveChangesAsync();
        return execution;
    }

    public async Task UpdateExecutionStatusAsync(Guid executionId, WorkflowExecutionStatus status, string? errorMessage = null)
    {
        var execution = await _context.AiEngineWorkflowExecutions.FindAsync(executionId);
        if (execution != null)
        {
            execution.Status = status.ToString();
            if (status == WorkflowExecutionStatus.Completed || status == WorkflowExecutionStatus.Failed)
            {
                execution.CompletedAt = DateTime.UtcNow;
            }
            if (!string.IsNullOrEmpty(errorMessage))
            {
                execution.ErrorMessage = errorMessage;
            }
            await _context.SaveChangesAsync();
        }
    }

    public async Task<WorkflowExecutionStep> StartStepAsync(Guid executionId, Guid workflowStepId)
    {
        var step = new WorkflowExecutionStep
        {
            ExecutionId = executionId,
            WorkflowStepId = workflowStepId,
            StartedAt = DateTime.UtcNow,
            Status = WorkflowExecutionStatus.InProgress.ToString()
        };

        _context.AiEngineWorkflowExecutionSteps.Add(step);
        await _context.SaveChangesAsync();
        return step;
    }

    public async Task UpdateStepStatusAsync(Guid stepId, WorkflowExecutionStatus status, string? output = null, string? errorMessage = null)
    {
        var step = await _context.AiEngineWorkflowExecutionSteps.FindAsync(stepId);
        if (step != null)
        {
            step.Status = status.ToString();
            if (status == WorkflowExecutionStatus.Completed || status == WorkflowExecutionStatus.Failed)
            {
                step.CompletedAt = DateTime.UtcNow;
            }
            if (!string.IsNullOrEmpty(output))
            {
                step.Output = output;
            }
            if (!string.IsNullOrEmpty(errorMessage))
            {
                step.ErrorMessage = errorMessage;
            }
            await _context.SaveChangesAsync();
        }
    }
}
