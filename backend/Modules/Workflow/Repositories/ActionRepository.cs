using backend.Modules.Workflow.Entities;
using backend.Modules.Workflow.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Workflow.Repositories;

public class ActionRepository : IActionRepository
{
    private readonly ApplicationDbContext _context;

    public ActionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<WorkflowAction>> GetByWorkflowIdAsync(Guid workflowId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkflowActions
            .Include(a => a.Conditions)
            .Where(a => a.WorkflowId == workflowId && !a.IsDeleted)
            .OrderBy(a => a.ExecutionOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<WorkflowAction> actions, CancellationToken cancellationToken = default)
    {
        await _context.WorkflowActions.AddRangeAsync(actions, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByWorkflowIdAsync(Guid workflowId, CancellationToken cancellationToken = default)
    {
        var actions = await _context.WorkflowActions.Where(a => a.WorkflowId == workflowId).ToListAsync(cancellationToken);
        _context.WorkflowActions.RemoveRange(actions);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
