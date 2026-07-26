using backend.Modules.Documents.Entities;

namespace backend.Modules.Documents.AuditLogs.Interfaces;

public interface IDocumentAuditLogRepository
{
    Task AddAsync(DocumentAuditEntry entry);

    Task<List<DocumentAuditEntry>> GetByDocumentIdAsync(Guid organizationId, Guid documentId, int limit = 100);

    Task<(List<DocumentAuditEntry> Items, int TotalCount)> GetPagedAsync(
        Guid organizationId,
        string? entityType,
        string? action,
        int page = 1,
        int pageSize = 20);
}
