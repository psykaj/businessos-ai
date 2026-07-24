using backend.Modules.AiAgent.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AiAgent.Repositories;

public class CommandExecutionRepository : ICommandExecutionRepository
{
    private readonly ApplicationDbContext _context;

    public CommandExecutionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CommandExecution?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.CommandExecutions
            .FirstOrDefaultAsync(ce => ce.Id == id && ce.OrganizationId == organizationId && !ce.IsDeleted, cancellationToken);
    }

    public async Task<List<CommandExecution>> GetRecentByOrganizationIdAsync(Guid organizationId, int limit = 20, CancellationToken cancellationToken = default)
    {
        return await _context.CommandExecutions
            .AsNoTracking()
            .Where(ce => ce.OrganizationId == organizationId && !ce.IsDeleted)
            .OrderByDescending(ce => ce.StartedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<(List<CommandExecution> Items, int TotalCount)> GetPagedAsync(
        Guid organizationId,
        int page = 1,
        int pageSize = 20,
        string? status = null,
        string? tool = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.CommandExecutions.AsNoTracking()
            .Where(ce => ce.OrganizationId == organizationId && !ce.IsDeleted);

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(ce => ce.Status.ToLower() == status.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(tool))
        {
            query = query.Where(ce => ce.ToolInvoked.ToLower() == tool.ToLower());
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(ce => ce.StartedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(CommandExecution execution, CancellationToken cancellationToken = default)
    {
        await _context.CommandExecutions.AddAsync(execution, cancellationToken);
    }

    public void Update(CommandExecution execution)
    {
        _context.CommandExecutions.Update(execution);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
