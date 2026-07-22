using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.Entities;
using backend.Modules.Workflow.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Workflow.Repositories;

public class WorkflowRepository : IWorkflowRepository
{
    private readonly ApplicationDbContext _context;

    public WorkflowRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Entities.Workflow?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Workflows
            .Include(w => w.Trigger)
            .Include(w => w.Actions.OrderBy(a => a.ExecutionOrder))
                .ThenInclude(a => a.Conditions)
            .Include(w => w.Conditions)
            .FirstOrDefaultAsync(w => w.Id == id && w.OrganizationId == organizationId && !w.IsDeleted, cancellationToken);
    }

    public async Task<List<Entities.Workflow>> GetByOrganizationIdAsync(
        Guid organizationId, int pageNumber, int pageSize, string? search, WorkflowStatus? status, CancellationToken cancellationToken = default)
    {
        var query = _context.Workflows
            .Include(w => w.Trigger)
            .Include(w => w.Actions.OrderBy(a => a.ExecutionOrder))
            .Where(w => w.OrganizationId == organizationId && !w.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(w => w.Name.Contains(search) || (w.Description != null && w.Description.Contains(search)));
        }

        if (status.HasValue)
        {
            query = query.Where(w => w.Status == status.Value);
        }

        return await query
            .OrderByDescending(w => w.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(
        Guid organizationId, string? search, WorkflowStatus? status, CancellationToken cancellationToken = default)
    {
        var query = _context.Workflows.Where(w => w.OrganizationId == organizationId && !w.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(w => w.Name.Contains(search) || (w.Description != null && w.Description.Contains(search)));
        }

        if (status.HasValue)
        {
            query = query.Where(w => w.Status == status.Value);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<List<Entities.Workflow>> GetActiveWorkflowsByTriggerTypeAsync(
        Guid organizationId, TriggerType triggerType, CancellationToken cancellationToken = default)
    {
        return await _context.Workflows
            .Include(w => w.Trigger)
            .Include(w => w.Actions.OrderBy(a => a.ExecutionOrder))
                .ThenInclude(a => a.Conditions)
            .Include(w => w.Conditions)
            .Where(w => w.OrganizationId == organizationId
                        && w.Status == WorkflowStatus.Active
                        && !w.IsDeleted
                        && w.Trigger != null
                        && w.Trigger.Enabled
                        && w.Trigger.TriggerType == triggerType)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Entities.Workflow>> GetActiveWorkflowsByTriggerTypeGlobalAsync(
        TriggerType triggerType, CancellationToken cancellationToken = default)
    {
        return await _context.Workflows
            .Include(w => w.Trigger)
            .Include(w => w.Actions.OrderBy(a => a.ExecutionOrder))
                .ThenInclude(a => a.Conditions)
            .Include(w => w.Conditions)
            .Where(w => w.Status == WorkflowStatus.Active
                        && !w.IsDeleted
                        && w.Trigger != null
                        && w.Trigger.Enabled
                        && w.Trigger.TriggerType == triggerType)
            .ToListAsync(cancellationToken);
    }

    public async Task<Entities.Workflow> AddAsync(Entities.Workflow workflow, CancellationToken cancellationToken = default)
    {
        await _context.Workflows.AddAsync(workflow, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return workflow;
    }

    public async Task UpdateAsync(Entities.Workflow workflow, CancellationToken cancellationToken = default)
    {
        workflow.UpdatedAt = DateTime.UtcNow;
        _context.Workflows.Update(workflow);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Entities.Workflow workflow, CancellationToken cancellationToken = default)
    {
        workflow.IsDeleted = true;
        workflow.DeletedAt = DateTime.UtcNow;
        _context.Workflows.Update(workflow);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
