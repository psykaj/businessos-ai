using System;
using System.Collections.Generic;
using MediatR;
using backend.Modules.Automation.Domain.Entities;
using backend.Modules.Automation.Domain.Enums;

namespace backend.Modules.Automation.Application.Commands;

public class CreateWorkflowCommand : IRequest<AiWorkflow>
{
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public WorkflowTriggerType TriggerType { get; set; }
    public int Priority { get; set; }
    public bool RequiresApproval { get; set; }
    public List<CreateWorkflowStepDto> Steps { get; set; } = new();
}

public class CreateWorkflowStepDto
{
    public int StepOrder { get; set; }
    public WorkflowStepType StepType { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Configuration { get; set; }
    public string? Condition { get; set; }
    public bool IsRequired { get; set; }
}

public class ActivateWorkflowCommand : IRequest<bool>
{
    public Guid WorkflowId { get; set; }
    public Guid OrganizationId { get; set; }
}

public class DeactivateWorkflowCommand : IRequest<bool>
{
    public Guid WorkflowId { get; set; }
    public Guid OrganizationId { get; set; }
}

public class DeleteWorkflowCommand : IRequest<bool>
{
    public Guid WorkflowId { get; set; }
    public Guid OrganizationId { get; set; }
}

public class ApproveWorkflowActionCommand : IRequest<bool>
{
    public Guid ExecutionStepId { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid UserId { get; set; }
}

public class RejectWorkflowActionCommand : IRequest<bool>
{
    public Guid ExecutionStepId { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid UserId { get; set; }
}
