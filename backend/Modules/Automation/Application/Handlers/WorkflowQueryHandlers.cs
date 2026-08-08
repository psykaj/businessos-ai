using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Modules.Automation.Domain.Entities;
using backend.Modules.Automation.Application.Queries;

namespace backend.Modules.Automation.Application.Handlers;

public class WorkflowQueryHandlers : 
    IRequestHandler<GetWorkflowsQuery, List<AiWorkflow>>,
    IRequestHandler<GetWorkflowByIdQuery, AiWorkflow?>,
    IRequestHandler<GetWorkflowExecutionsQuery, List<WorkflowExecution>>,
    IRequestHandler<GetWorkflowExecutionByIdQuery, WorkflowExecution?>,
    IRequestHandler<GetWorkflowTemplatesQuery, List<WorkflowTemplate>>
{
    private readonly ApplicationDbContext _context;

    public WorkflowQueryHandlers(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<AiWorkflow>> Handle(GetWorkflowsQuery request, CancellationToken cancellationToken)
    {
        return _context.AiEngineWorkflows
            .Include(w => w.Steps)
            .Where(w => w.OrganizationId == request.OrganizationId && !w.IsDeleted)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<AiWorkflow?> Handle(GetWorkflowByIdQuery request, CancellationToken cancellationToken)
    {
        return _context.AiEngineWorkflows
            .Include(w => w.Steps)
            .FirstOrDefaultAsync(w => w.Id == request.WorkflowId && w.OrganizationId == request.OrganizationId && !w.IsDeleted, cancellationToken);
    }

    public Task<List<WorkflowExecution>> Handle(GetWorkflowExecutionsQuery request, CancellationToken cancellationToken)
    {
        return _context.AiEngineWorkflowExecutions
            .Where(e => e.OrganizationId == request.OrganizationId)
            .OrderByDescending(e => e.StartedAt)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);
    }

    public Task<WorkflowExecution?> Handle(GetWorkflowExecutionByIdQuery request, CancellationToken cancellationToken)
    {
        return _context.AiEngineWorkflowExecutions
            .Include(e => e.ExecutionSteps)
            .ThenInclude(es => es.WorkflowStep)
            .FirstOrDefaultAsync(e => e.Id == request.ExecutionId && e.OrganizationId == request.OrganizationId, cancellationToken);
    }

    public Task<List<WorkflowTemplate>> Handle(GetWorkflowTemplatesQuery request, CancellationToken cancellationToken)
    {
        return _context.AiEngineWorkflowTemplates
            .Where(t => t.IsActive && !t.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}
