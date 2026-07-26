using backend.Modules.Documents.AuditLogs.DTOs;

namespace backend.Modules.Documents.AuditLogs.Interfaces;

public interface IDocumentAuditLogService
{
    Task LogAsync(Guid organizationId, CreateAuditLogDto dto);
    Task<List<DocumentAuditEntryDto>> GetByDocumentIdAsync(Guid organizationId, Guid documentId, int limit = 100);
    Task<(List<DocumentAuditEntryDto> Items, int TotalCount)> GetPagedAsync(Guid organizationId, string? entityType, string? action, int page = 1, int pageSize = 20);
}
