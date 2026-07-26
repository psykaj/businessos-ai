namespace backend.Modules.Documents.Sharing.DTOs;

public record SharedDocumentDto(
    Guid Id,
    Guid OrganizationId,
    Guid DocumentId,
    string DocumentName,
    Guid? SharedWithUserId,
    string? SharedWithEmail,
    string AccessLevel,
    string PermissionType,
    string? PublicShareToken,
    string? ShareUrl,
    bool HasPasscode,
    DateTime? ExpirationDate,
    int AccessCount,
    bool IsActive,
    Guid SharedById,
    DateTime CreatedAt
);

public record ShareDocumentDto(
    Guid DocumentId,
    Guid? SharedWithUserId = null,
    string? SharedWithEmail = null,
    string AccessLevel = "Read",
    string PermissionType = "InternalUser",
    string? Passcode = null,
    DateTime? ExpirationDate = null
);

public record PublicAccessDto(
    string? Passcode = null
);
