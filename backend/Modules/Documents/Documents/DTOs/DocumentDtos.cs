namespace backend.Modules.Documents.Documents.DTOs;

public record DocumentDto(
    Guid Id,
    Guid OrganizationId,
    Guid? FolderId,
    string Name,
    string? Description,
    string MimeType,
    string FileExtension,
    long FileSize,
    string FilePath,
    string StorageProvider,
    string Status,
    Guid OwnerId,
    Guid? CurrentVersionId,
    int VersionCount,
    bool IsFavorite,
    List<string> Tags,
    string? Metadata,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record DocumentVersionDto(
    Guid Id,
    Guid OrganizationId,
    Guid DocumentId,
    int VersionNumber,
    string FilePath,
    string StorageProvider,
    long FileSize,
    string MimeType,
    string? ChangesSummary,
    Guid? UploadedById,
    string? UploadedByName,
    DateTime CreatedAt
);

public record DocumentPreviewDto(
    Guid DocumentId,
    string Name,
    string MimeType,
    long FileSize,
    string PreviewUrl,
    string DownloadUrl,
    bool CanPreviewInline
);

public record UploadDocumentDto(
    Guid? FolderId,
    string? Description,
    List<string>? Tags,
    string? Metadata
);

public record RenameDocumentDto(
    string NewName
);

public record MoveDocumentDto(
    Guid? TargetFolderId
);

public record CopyDocumentDto(
    Guid? TargetFolderId,
    string? NewName
);

public record UpdateTagsDto(
    List<string> Tags
);

public record DocumentSearchQueryDto(
    string? Query,
    Guid? FolderId,
    string? Status,
    string? Tag,
    string? MimeType,
    bool? IsFavorite,
    bool IncludeDeleted = false,
    int Page = 1,
    int PageSize = 20,
    string SortBy = "UpdatedAt",
    bool Descending = true
);
