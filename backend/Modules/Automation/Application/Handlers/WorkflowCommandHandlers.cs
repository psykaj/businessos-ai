using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Modules.Automation.Domain.Entities;
using backend.Modules.Automation.Application.Commands;
using backend.Modules.Automation.Domain.Enums;
using backend.Modules.Automation.Application.Interfaces;

namespace backend.Modules.Automation.Application.Handlers;

public class WorkflowCommandHandlers : 
    IRequestHandler<CreateWorkflowCommand, AiWorkflow>,
    IRequestHandler<ActivateWorkflowCommand, bool>,
    IRequestHandler<DeactivateWorkflowCommand, bool>,
    IRequestHandler<DeleteWorkflowCommand, bool>,
    IRequestHandler<ApproveWorkflowActionCommand, bool>,
    IRequestHandler<RejectWorkflowActionCommand, bool>
{
    private readonly ApplicationDbContext _context;
    private readonly IWorkflowApprovalService _approvalService;

    public WorkflowCommandHandlers(ApplicationDbContext context, IWorkflowApprovalService approvalService)
    {
        _context = context;
        _approvalService = approvalService;
    }

    public async Task<AiWorkflow> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
    {
        var workflow = new AiWorkflow
        {
            OrganizationId = request.OrganizationId,
            Name = request.Name,
            Description = request.Description,
            TriggerType = request.TriggerType.ToString(),
            Priority = request.Priority,
            RequiresApproval = request.RequiresApproval,
            Status = WorkflowStatus.Draft.ToString(),
            IsActive = false
        };

        foreach (var stepDto in request.Steps)
        {
            var step = new WorkflowStep
            {
                StepOrder = stepDto.StepOrder,
                StepType = stepDto.StepType.ToString(),
                Name = stepDto.Name,
                Configuration = stepDto.Configuration,
                Condition = stepDto.Condition,
                IsRequired = stepDto.IsRequired,
                Status = "Active"
            };
            workflow.Steps.Add(step);
        }

        _context.AiEngineWorkflows.Add(workflow);
        await _context.SaveChangesAsync(cancellationToken);

        return workflow;
    }

    public async Task<bool> Handle(ActivateWorkflowCommand request, CancellationToken cancellationToken)
    {
        var workflow = await _context.AiEngineWorkflows
            .FirstOrDefaultAsync(w => w.Id == request.WorkflowId && w.OrganizationId == request.OrganizationId, cancellationToken);
            
        if (workflow == null) return false;

        workflow.IsActive = true;
        workflow.Status = WorkflowStatus.Active.ToString();
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    public async Task<bool> Handle(DeactivateWorkflowCommand request, CancellationToken cancellationToken)
    {
        var workflow = await _context.AiEngineWorkflows
            .FirstOrDefaultAsync(w => w.Id == request.WorkflowId && w.OrganizationId == request.OrganizationId, cancellationToken);
            
        if (workflow == null) return false;

        workflow.IsActive = false;
        workflow.Status = WorkflowStatus.Inactive.ToString();
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    public async Task<bool> Handle(DeleteWorkflowCommand request, CancellationToken cancellationToken)
    {
        var workflow = await _context.AiEngineWorkflows
            .FirstOrDefaultAsync(w => w.Id == request.WorkflowId && w.OrganizationId == request.OrganizationId, cancellationToken);
            
        if (workflow == null) return false;

        _context.AiEngineWorkflows.Remove(workflow);
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    public async Task<bool> Handle(ApproveWorkflowActionCommand request, CancellationToken cancellationToken)
    {
        await _approvalService.ApproveActionAsync(request.ExecutionStepId, request.OrganizationId, request.UserId);
        return true;
    }

    public async Task<bool> Handle(RejectWorkflowActionCommand request, CancellationToken cancellationToken)
    {
        await _approvalService.RejectActionAsync(request.ExecutionStepId, request.OrganizationId, request.UserId);
        return true;
    }
}
