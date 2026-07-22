using backend.Modules.Workflow.Entities;
using backend.Modules.Workflow.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Workflow.Repositories;

public class TriggerRepository : ITriggerRepository
{
    private readonly ApplicationDbContext _context;

    public TriggerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkflowTrigger?> GetByWorkflowIdAsync(Guid workflowId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkflowTriggers
            .FirstOrDefaultAsync(t => t.WorkflowId == workflowId && !t.IsDeleted, cancellationToken);
    }

    public async Task AddAsync(WorkflowTrigger trigger, CancellationToken cancellationToken = default)
    {
        await _context.WorkflowTriggers.AddAsync(trigger, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(WorkflowTrigger trigger, CancellationToken cancellationToken = default)
    {
        trigger.UpdatedAt = DateTime.UtcNow;
        _context.WorkflowTriggers.Update(trigger);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(WorkflowTrigger trigger, CancellationToken cancellationToken = default)
    {
        trigger.IsDeleted = true;
        trigger.DeletedAt = DateTime.UtcNow;
        _context.WorkflowTriggers.Update(trigger);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
