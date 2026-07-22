using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.Entities;
using backend.Modules.Workflow.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Workflow.Repositories;

public class ExecutionRepository : IExecutionRepository
{
    private readonly ApplicationDbContext _context;

    public ExecutionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkflowExecution?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkflowExecutions
            .Include(e => e.Logs)
            .FirstOrDefaultAsync(e => e.Id == id && e.OrganizationId == organizationId && !e.IsDeleted, cancellationToken);
    }

    public async Task<List<WorkflowExecution>> GetByWorkflowIdAsync(
        Guid workflowId, Guid organizationId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.WorkflowExecutions
            .Include(e => e.Logs)
            .Where(e => e.WorkflowId == workflowId && e.OrganizationId == organizationId && !e.IsDeleted)
            .OrderByDescending(e => e.StartedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountByWorkflowIdAsync(Guid workflowId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkflowExecutions
            .Where(e => e.WorkflowId == workflowId && e.OrganizationId == organizationId && !e.IsDeleted)
            .CountAsync(cancellationToken);
    }

    public async Task<List<WorkflowExecution>> GetByOrganizationIdAsync(
        Guid organizationId, int pageNumber, int pageSize, WorkflowExecutionStatus? status, CancellationToken cancellationToken = default)
    {
        var query = _context.WorkflowExecutions
            .Include(e => e.Workflow)
            .Where(e => e.OrganizationId == organizationId && !e.IsDeleted);

        if (status.HasValue)
        {
            query = query.Where(e => e.Status == status.Value);
        }

        return await query
            .OrderByDescending(e => e.StartedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountByOrganizationIdAsync(Guid organizationId, WorkflowExecutionStatus? status, CancellationToken cancellationToken = default)
    {
        var query = _context.WorkflowExecutions.Where(e => e.OrganizationId == organizationId && !e.IsDeleted);

        if (status.HasValue)
        {
            query = query.Where(e => e.Status == status.Value);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<WorkflowExecution> AddAsync(WorkflowExecution execution, CancellationToken cancellationToken = default)
    {
        await _context.WorkflowExecutions.AddAsync(execution, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return execution;
    }

    public async Task UpdateAsync(WorkflowExecution execution, CancellationToken cancellationToken = default)
    {
        execution.UpdatedAt = DateTime.UtcNow;
        _context.WorkflowExecutions.Update(execution);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
