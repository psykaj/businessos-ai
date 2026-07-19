using backend.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AuditLogs.Repositories;

public class AuditLogRepository : backend.Repositories.GenericRepository<AuditLog>, IAuditLogRepository
{
    private readonly ApplicationDbContext _context;

    public AuditLogRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<AuditLog> Items, int TotalCount)> GetPagedAsync(
        Guid organizationId, int page, int pageSize, string? module, string? action, CancellationToken cancellationToken = default)
    {
        var query = _context.AuditLogs
            .Include(a => a.User)
            .Where(a => a.OrganizationId == organizationId && !a.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(module))
        {
            query = query.Where(a => a.Module == module);
        }

        if (!string.IsNullOrWhiteSpace(action))
        {
            query = query.Where(a => a.Action == action);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
