using System.Text.Json;
using backend.Modules.Documents.AuditLogs.DTOs;
using backend.Modules.Documents.AuditLogs.Interfaces;
using backend.Modules.Documents.Documents.DTOs;
using backend.Modules.Documents.Documents.Interfaces;
using backend.Modules.Documents.Entities;
using backend.Modules.Documents.Storage.Interfaces;

namespace backend.Modules.Documents.Documents.Services;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _repository;
    private readonly IStorageService _storageService;
    private readonly IDocumentAuditLogService _auditLogService;

    public DocumentService(
        IDocumentRepository repository,
        IStorageService storageService,
        IDocumentAuditLogService auditLogService)
    {
        _repository = repository;
        _storageService = storageService;
        _auditLogService = auditLogService;
    }

    public async Task<DocumentDto> UploadAsync(
        Guid organizationId,
        Guid ownerId,
        string ownerName,
        Stream stream,
        string fileName,
        string contentType,
        UploadDocumentDto dto)
    {
        var cleanFileName = Path.GetFileName(fileName);
        var extension = Path.GetExtension(cleanFileName);
        var relativePath = await _storageService.UploadAsync(stream, cleanFileName, contentType, organizationId);

        var document = new Document
        {
            OrganizationId = organizationId,
            FolderId = dto.FolderId,
            Name = cleanFileName,
            Description = dto.Description,
            MimeType = contentType,
            FileExtension = extension,
            FileSize = stream.Length,
            FilePath = relativePath,
            StorageProvider = _storageService.StorageProviderName,
            Status = "Active",
            OwnerId = ownerId,
            VersionCount = 1,
            Tags = dto.Tags != null ? JsonSerializer.Serialize(dto.Tags) : null,
            Metadata = dto.Metadata
        };

        var initialVersion = new DocumentVersion
        {
            OrganizationId = organizationId,
            DocumentId = document.Id,
            VersionNumber = 1,
            FilePath = relativePath,
            StorageProvider = _storageService.StorageProviderName,
            FileSize = stream.Length,
            MimeType = contentType,
            ChangesSummary = "Initial upload",
            UploadedById = ownerId,
            UploadedByName = ownerName
        };

        document.CurrentVersionId = initialVersion.Id;
        document.Versions.Add(initialVersion);

        await _repository.AddAsync(document);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            document.Id, "Document", "Uploaded", ownerId, ownerName, null,
            JsonSerializer.Serialize(new { FileName = cleanFileName, FileSize = stream.Length })
        ));

        return MapToDto(document);
    }

    public async Task<DocumentDto> UploadNewVersionAsync(
        Guid organizationId,
        Guid documentId,
        Guid userId,
        string userName,
        Stream stream,
        string fileName,
        string contentType,
        string? changesSummary)
    {
        var document = await _repository.GetByIdAsync(organizationId, documentId)
            ?? throw new KeyNotFoundException($"Document with ID {documentId} not found.");

        var cleanFileName = Path.GetFileName(fileName);
        var relativePath = await _storageService.UploadAsync(stream, cleanFileName, contentType, organizationId);

        document.VersionCount++;
        var newVersion = new DocumentVersion
        {
            OrganizationId = organizationId,
            DocumentId = document.Id,
            VersionNumber = document.VersionCount,
            FilePath = relativePath,
            StorageProvider = _storageService.StorageProviderName,
            FileSize = stream.Length,
            MimeType = contentType,
            ChangesSummary = changesSummary ?? $"Version {document.VersionCount} uploaded",
            UploadedById = userId,
            UploadedByName = userName
        };

        document.FilePath = relativePath;
        document.FileSize = stream.Length;
        document.MimeType = contentType;
        document.CurrentVersionId = newVersion.Id;
        document.Versions.Add(newVersion);

        await _repository.UpdateAsync(document);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            document.Id, "Document", "VersionCreated", userId, userName, null,
            JsonSerializer.Serialize(new { VersionNumber = document.VersionCount, ChangesSummary = changesSummary })
        ));

        return MapToDto(document);
    }

    public async Task<DocumentDto> GetByIdAsync(Guid organizationId, Guid id)
    {
        var document = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Document with ID {id} not found.");

        return MapToDto(document);
    }

    public async Task<(Stream Stream, string ContentType, string FileName)> DownloadAsync(Guid organizationId, Guid id, Guid? versionId = null)
    {
        var document = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Document with ID {id} not found.");

        string targetPath = document.FilePath;
        string targetMimeType = document.MimeType;

        if (versionId.HasValue)
        {
            var version = await _repository.GetVersionByIdAsync(organizationId, versionId.Value)
                ?? throw new KeyNotFoundException($"Version {versionId} not found for document {id}.");
            targetPath = version.FilePath;
            targetMimeType = version.MimeType;
        }

        var fileStream = await _storageService.DownloadAsync(targetPath);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            document.Id, "Document", "Downloaded", null, null, null,
            JsonSerializer.Serialize(new { VersionId = versionId })
        ));

        return (fileStream, targetMimeType, document.Name);
    }

    public async Task<DocumentPreviewDto> GetPreviewAsync(Guid organizationId, Guid id)
    {
        var document = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Document with ID {id} not found.");

        var presignedUrl = await _storageService.GetPresignedUrlAsync(document.FilePath, TimeSpan.FromMinutes(30));
        var downloadUrl = $"/api/documents/{document.Id}/download";

        bool canPreviewInline = document.MimeType.StartsWith("image/") ||
                               document.MimeType == "application/pdf" ||
                               document.MimeType.StartsWith("text/");

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            document.Id, "Document", "Viewed", null, null, null, null
        ));

        return new DocumentPreviewDto(
            document.Id,
            document.Name,
            document.MimeType,
            document.FileSize,
            presignedUrl,
            downloadUrl,
            canPreviewInline
        );
    }

    public async Task<DocumentDto> RenameAsync(Guid organizationId, Guid id, RenameDocumentDto dto)
    {
        var document = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Document with ID {id} not found.");

        var oldName = document.Name;
        document.Name = dto.NewName.Trim();

        await _repository.UpdateAsync(document);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            document.Id, "Document", "Renamed", null, null, null,
            JsonSerializer.Serialize(new { OldName = oldName, NewName = document.Name })
        ));

        return MapToDto(document);
    }

    public async Task<DocumentDto> MoveAsync(Guid organizationId, Guid id, MoveDocumentDto dto)
    {
        var document = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Document with ID {id} not found.");

        var oldFolderId = document.FolderId;
        document.FolderId = dto.TargetFolderId;

        await _repository.UpdateAsync(document);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            document.Id, "Document", "Moved", null, null, null,
            JsonSerializer.Serialize(new { OldFolderId = oldFolderId, TargetFolderId = dto.TargetFolderId })
        ));

        return MapToDto(document);
    }

    public async Task<DocumentDto> CopyAsync(Guid organizationId, Guid id, CopyDocumentDto dto)
    {
        var sourceDoc = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Source document with ID {id} not found.");

        var newName = dto.NewName?.Trim() ?? $"Copy of {sourceDoc.Name}";
        var newFileName = $"{Guid.NewGuid():N}_{newName}";
        var newPath = $"{organizationId}/{newFileName}";

        await _storageService.CopyAsync(sourceDoc.FilePath, newPath);

        var copiedDocument = new Document
        {
            OrganizationId = organizationId,
            FolderId = dto.TargetFolderId ?? sourceDoc.FolderId,
            Name = newName,
            Description = sourceDoc.Description,
            MimeType = sourceDoc.MimeType,
            FileExtension = sourceDoc.FileExtension,
            FileSize = sourceDoc.FileSize,
            FilePath = newPath,
            StorageProvider = sourceDoc.StorageProvider,
            Status = "Active",
            OwnerId = sourceDoc.OwnerId,
            VersionCount = 1,
            Tags = sourceDoc.Tags,
            Metadata = sourceDoc.Metadata
        };

        var initialVersion = new DocumentVersion
        {
            OrganizationId = organizationId,
            DocumentId = copiedDocument.Id,
            VersionNumber = 1,
            FilePath = newPath,
            StorageProvider = sourceDoc.StorageProvider,
            FileSize = sourceDoc.FileSize,
            MimeType = sourceDoc.MimeType,
            ChangesSummary = $"Copied from {sourceDoc.Name}",
            UploadedById = sourceDoc.OwnerId,
            UploadedByName = "System"
        };

        copiedDocument.CurrentVersionId = initialVersion.Id;
        copiedDocument.Versions.Add(initialVersion);

        await _repository.AddAsync(copiedDocument);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            copiedDocument.Id, "Document", "Copied", null, null, null,
            JsonSerializer.Serialize(new { SourceDocumentId = id })
        ));

        return MapToDto(copiedDocument);
    }

    public async Task SoftDeleteAsync(Guid organizationId, Guid id, Guid userId, string userName)
    {
        var document = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Document with ID {id} not found.");

        document.IsDeleted = true;
        document.DeletedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(document);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            document.Id, "Document", "Deleted", userId, userName, null, null
        ));
    }

    public async Task PermanentDeleteAsync(Guid organizationId, Guid id)
    {
        var document = await _repository.GetByIdAsync(organizationId, id, includeDeleted: true)
            ?? throw new KeyNotFoundException($"Document with ID {id} not found.");

        await _storageService.DeleteAsync(document.FilePath);

        document.IsDeleted = true;
        document.DeletedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(document);
    }

    public async Task<DocumentDto> RestoreAsync(Guid organizationId, Guid id)
    {
        var document = await _repository.GetByIdAsync(organizationId, id, includeDeleted: true)
            ?? throw new KeyNotFoundException($"Document with ID {id} not found.");

        document.IsDeleted = false;
        document.DeletedAt = null;

        await _repository.UpdateAsync(document);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            document.Id, "Document", "Restored", null, null, null, null
        ));

        return MapToDto(document);
    }

    public async Task<List<DocumentVersionDto>> GetVersionsAsync(Guid organizationId, Guid documentId)
    {
        var versions = await _repository.GetVersionsByDocumentIdAsync(organizationId, documentId);
        return versions.Select(v => new DocumentVersionDto(
            v.Id,
            v.OrganizationId,
            v.DocumentId,
            v.VersionNumber,
            v.FilePath,
            v.StorageProvider,
            v.FileSize,
            v.MimeType,
            v.ChangesSummary,
            v.UploadedById,
            v.UploadedByName,
            v.CreatedAt
        )).ToList();
    }

    public async Task<DocumentDto> RevertToVersionAsync(Guid organizationId, Guid documentId, Guid versionId, Guid userId, string userName)
    {
        var document = await _repository.GetByIdAsync(organizationId, id: documentId)
            ?? throw new KeyNotFoundException($"Document with ID {documentId} not found.");

        var targetVersion = await _repository.GetVersionByIdAsync(organizationId, versionId)
            ?? throw new KeyNotFoundException($"Version {versionId} not found.");

        document.FilePath = targetVersion.FilePath;
        document.FileSize = targetVersion.FileSize;
        document.MimeType = targetVersion.MimeType;
        document.CurrentVersionId = targetVersion.Id;

        await _repository.UpdateAsync(document);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            document.Id, "Document", "VersionReverted", userId, userName, null,
            JsonSerializer.Serialize(new { RevertedToVersionNumber = targetVersion.VersionNumber })
        ));

        return MapToDto(document);
    }

    public async Task<(List<DocumentDto> Items, int TotalCount)> SearchAsync(Guid organizationId, DocumentSearchQueryDto query)
    {
        var (items, totalCount) = await _repository.SearchAsync(organizationId, query);
        return (items.Select(MapToDto).ToList(), totalCount);
    }

    public async Task<DocumentDto> UpdateTagsAsync(Guid organizationId, Guid id, UpdateTagsDto dto)
    {
        var document = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Document with ID {id} not found.");

        document.Tags = dto.Tags != null ? JsonSerializer.Serialize(dto.Tags) : null;

        await _repository.UpdateAsync(document);
        return MapToDto(document);
    }

    public async Task<DocumentDto> ToggleFavoriteAsync(Guid organizationId, Guid id)
    {
        var document = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Document with ID {id} not found.");

        document.IsFavorite = !document.IsFavorite;

        await _repository.UpdateAsync(document);
        return MapToDto(document);
    }

    public async Task<List<DocumentDto>> GetFavoritesAsync(Guid organizationId)
    {
        var docs = await _repository.GetFavoritesAsync(organizationId);
        return docs.Select(MapToDto).ToList();
    }

    private static DocumentDto MapToDto(Document document)
    {
        List<string> tagsList = new();
        if (!string.IsNullOrWhiteSpace(document.Tags))
        {
            try
            {
                tagsList = JsonSerializer.Deserialize<List<string>>(document.Tags) ?? new();
            }
            catch { }
        }

        return new DocumentDto(
            document.Id,
            document.OrganizationId,
            document.FolderId,
            document.Name,
            document.Description,
            document.MimeType,
            document.FileExtension,
            document.FileSize,
            document.FilePath,
            document.StorageProvider,
            document.Status,
            document.OwnerId,
            document.CurrentVersionId,
            document.VersionCount,
            document.IsFavorite,
            tagsList,
            document.Metadata,
            document.CreatedAt,
            document.UpdatedAt
        );
    }
}
