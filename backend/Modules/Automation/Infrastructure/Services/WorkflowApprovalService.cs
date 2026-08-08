using System;
using System.Threading.Tasks;
using backend.Persistence;
using backend.Modules.Automation.Application.Interfaces;
using backend.Modules.Automation.Domain.Enums;
using backend.Modules.Automation.Domain.Entities;

namespace backend.Modules.Automation.Infrastructure.Services;

public class WorkflowApprovalService : IWorkflowApprovalService
{
    private readonly ApplicationDbContext _context;
    private readonly IWorkflowExecutionLogger _logger;

    public WorkflowApprovalService(ApplicationDbContext context, IWorkflowExecutionLogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task RequestApprovalAsync(WorkflowStep step, Guid executionId, Guid organizationId)
    {
        // This creates a pending step in the execution logs and could optionally notify Action Center or Users
        var executionStep = await _logger.StartStepAsync(executionId, step.Id);
        await _logger.UpdateStepStatusAsync(executionStep.Id, WorkflowExecutionStatus.WaitingForApproval, "Pending manual approval");
        await _logger.UpdateExecutionStatusAsync(executionId, WorkflowExecutionStatus.WaitingForApproval);
        
        // TODO: Send notification or create task in Action Center for approval
    }

    public async Task ApproveActionAsync(Guid executionStepId, Guid organizationId, Guid approvedBy)
    {
        var step = await _context.AiEngineWorkflowExecutionSteps.FindAsync(executionStepId);
        if (step != null && step.Status == WorkflowExecutionStatus.WaitingForApproval.ToString())
        {
            await _logger.UpdateStepStatusAsync(executionStepId, WorkflowExecutionStatus.Completed, $"Approved by {approvedBy}");
            
            // Resume workflow... this would ideally publish an event to the engine to continue execution
            // For now, we update the status, and the engine could be triggered via an endpoint
        }
    }

    public async Task RejectActionAsync(Guid executionStepId, Guid organizationId, Guid rejectedBy)
    {
        var step = await _context.AiEngineWorkflowExecutionSteps.FindAsync(executionStepId);
        if (step != null && step.Status == WorkflowExecutionStatus.WaitingForApproval.ToString())
        {
            await _logger.UpdateStepStatusAsync(executionStepId, WorkflowExecutionStatus.Cancelled, $"Rejected by {rejectedBy}");
            await _logger.UpdateExecutionStatusAsync(step.ExecutionId, WorkflowExecutionStatus.Cancelled, "Workflow rejected at approval step");
        }
    }
}
