using backend.Modules.Documents.AuditLogs.DTOs;
using backend.Modules.Documents.AuditLogs.Interfaces;
using backend.Modules.Documents.Entities;

namespace backend.Modules.Documents.AuditLogs.Services;

public class DocumentAuditLogService : IDocumentAuditLogService
{
    private readonly IDocumentAuditLogRepository _repository;

    public DocumentAuditLogService(IDocumentAuditLogRepository repository)
    {
        _repository = repository;
    }

    public async Task LogAsync(Guid organizationId, CreateAuditLogDto dto)
    {
        var entry = new DocumentAuditEntry
        {
            OrganizationId = organizationId,
            DocumentId = dto.DocumentId,
            EntityType = dto.EntityType,
            Action = dto.Action,
            PerformedById = dto.PerformedById,
            PerformedByName = dto.PerformedByName,
            IpAddress = dto.IpAddress,
            DetailsJson = dto.DetailsJson,
            Timestamp = DateTime.UtcNow
        };

        await _repository.AddAsync(entry);
    }

    public async Task<List<DocumentAuditEntryDto>> GetByDocumentIdAsync(Guid organizationId, Guid documentId, int limit = 100)
    {
        var logs = await _repository.GetByDocumentIdAsync(organizationId, documentId, limit);
        return logs.Select(ToDto).ToList();
    }

    public async Task<(List<DocumentAuditEntryDto> Items, int TotalCount)> GetPagedAsync(Guid organizationId, string? entityType, string? action, int page = 1, int pageSize = 20)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(organizationId, entityType, action, page, pageSize);
        return (items.Select(ToDto).ToList(), totalCount);
    }

    private static DocumentAuditEntryDto ToDto(DocumentAuditEntry entry) => new(
        entry.Id,
        entry.OrganizationId,
        entry.DocumentId,
        entry.EntityType,
        entry.Action,
        entry.PerformedById,
        entry.PerformedByName,
        entry.IpAddress,
        entry.DetailsJson,
        entry.Timestamp
    );
}
