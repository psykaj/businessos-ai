using backend.Modules.Documents.AuditLogs.Interfaces;
using backend.Modules.Documents.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Documents.AuditLogs.Repositories;

public class DocumentAuditLogRepository : IDocumentAuditLogRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DocumentAuditLogRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(DocumentAuditEntry entry)
    {
        await _dbContext.DocumentAuditEntries.AddAsync(entry);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<DocumentAuditEntry>> GetByDocumentIdAsync(Guid organizationId, Guid documentId, int limit = 100)
    {
        return await _dbContext.DocumentAuditEntries
            .AsNoTracking()
            .Where(a => a.OrganizationId == organizationId && a.DocumentId == documentId)
            .OrderByDescending(a => a.Timestamp)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<(List<DocumentAuditEntry> Items, int TotalCount)> GetPagedAsync(
        Guid organizationId,
        string? entityType,
        string? action,
        int page = 1,
        int pageSize = 20)
    {
        var query = _dbContext.DocumentAuditEntries
            .AsNoTracking()
            .Where(a => a.OrganizationId == organizationId);

        if (!string.IsNullOrWhiteSpace(entityType))
        {
            query = query.Where(a => a.EntityType.ToLower() == entityType.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(action))
        {
            query = query.Where(a => a.Action.ToLower() == action.ToLower());
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(a => a.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
