namespace backend.Modules.Documents.AuditLogs.DTOs;

public record DocumentAuditEntryDto(
    Guid Id,
    Guid OrganizationId,
    Guid? DocumentId,
    string EntityType,
    string Action,
    Guid? PerformedById,
    string? PerformedByName,
    string? IpAddress,
    string? DetailsJson,
    DateTime Timestamp
);

public record CreateAuditLogDto(
    Guid? DocumentId,
    string EntityType,
    string Action,
    Guid? PerformedById,
    string? PerformedByName,
    string? IpAddress,
    string? DetailsJson
);
