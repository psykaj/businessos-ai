namespace backend.Modules.Documents.Folders.DTOs;

public record FolderDto(
    Guid Id,
    Guid OrganizationId,
    Guid? ParentFolderId,
    string Name,
    string? Description,
    string Path,
    string? Color,
    string? Icon,
    int SubFolderCount,
    int DocumentCount,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record FolderTreeDto(
    Guid Id,
    Guid OrganizationId,
    Guid? ParentFolderId,
    string Name,
    string Path,
    string? Color,
    string? Icon,
    List<FolderTreeDto> Children
);

public record CreateFolderDto(
    Guid? ParentFolderId,
    string Name,
    string? Description,
    string? Color,
    string? Icon
);

public record UpdateFolderDto(
    string Name,
    string? Description,
    string? Color,
    string? Icon
);

public record MoveFolderDto(
    Guid? TargetParentFolderId
);
