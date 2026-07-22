using backend.Modules.Workflow.Entities;
using backend.Modules.Workflow.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Workflow.Repositories;

public class LogRepository : ILogRepository
{
    private readonly ApplicationDbContext _context;

    public LogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<WorkflowExecutionLog>> GetByExecutionIdAsync(Guid executionId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkflowExecutionLogs
            .Where(l => l.ExecutionId == executionId)
            .OrderBy(l => l.StartedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(WorkflowExecutionLog log, CancellationToken cancellationToken = default)
    {
        await _context.WorkflowExecutionLogs.AddAsync(log, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<WorkflowExecutionLog> logs, CancellationToken cancellationToken = default)
    {
        await _context.WorkflowExecutionLogs.AddRangeAsync(logs, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
