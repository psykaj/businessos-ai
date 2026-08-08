using System;
using System.Collections.Generic;
using MediatR;
using backend.Modules.Automation.Domain.Entities;

namespace backend.Modules.Automation.Application.Queries;

public class GetWorkflowsQuery : IRequest<List<AiWorkflow>>
{
    public Guid OrganizationId { get; set; }
}

public class GetWorkflowByIdQuery : IRequest<AiWorkflow?>
{
    public Guid WorkflowId { get; set; }
    public Guid OrganizationId { get; set; }
}

public class GetWorkflowExecutionsQuery : IRequest<List<WorkflowExecution>>
{
    public Guid OrganizationId { get; set; }
    public int Limit { get; set; } = 50;
}

public class GetWorkflowExecutionByIdQuery : IRequest<WorkflowExecution?>
{
    public Guid ExecutionId { get; set; }
    public Guid OrganizationId { get; set; }
}

public class GetWorkflowTemplatesQuery : IRequest<List<WorkflowTemplate>>
{
}
