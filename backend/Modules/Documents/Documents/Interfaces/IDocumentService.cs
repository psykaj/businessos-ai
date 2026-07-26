using backend.Modules.Documents.Documents.DTOs;

namespace backend.Modules.Documents.Documents.Interfaces;

public interface IDocumentService
{
    Task<DocumentDto> UploadAsync(Guid organizationId, Guid ownerId, string ownerName, Stream stream, string fileName, string contentType, UploadDocumentDto dto);
    Task<DocumentDto> UploadNewVersionAsync(Guid organizationId, Guid documentId, Guid userId, string userName, Stream stream, string fileName, string contentType, string? changesSummary);
    Task<DocumentDto> GetByIdAsync(Guid organizationId, Guid id);
    Task<(Stream Stream, string ContentType, string FileName)> DownloadAsync(Guid organizationId, Guid id, Guid? versionId = null);
    Task<DocumentPreviewDto> GetPreviewAsync(Guid organizationId, Guid id);
    Task<DocumentDto> RenameAsync(Guid organizationId, Guid id, RenameDocumentDto dto);
    Task<DocumentDto> MoveAsync(Guid organizationId, Guid id, MoveDocumentDto dto);
    Task<DocumentDto> CopyAsync(Guid organizationId, Guid id, CopyDocumentDto dto);
    Task SoftDeleteAsync(Guid organizationId, Guid id, Guid userId, string userName);
    Task PermanentDeleteAsync(Guid organizationId, Guid id);
    Task<DocumentDto> RestoreAsync(Guid organizationId, Guid id);
    Task<List<DocumentVersionDto>> GetVersionsAsync(Guid organizationId, Guid documentId);
    Task<DocumentDto> RevertToVersionAsync(Guid organizationId, Guid documentId, Guid versionId, Guid userId, string userName);
    Task<(List<DocumentDto> Items, int TotalCount)> SearchAsync(Guid organizationId, DocumentSearchQueryDto query);
    Task<DocumentDto> UpdateTagsAsync(Guid organizationId, Guid id, UpdateTagsDto dto);
    Task<DocumentDto> ToggleFavoriteAsync(Guid organizationId, Guid id);
    Task<List<DocumentDto>> GetFavoritesAsync(Guid organizationId);
}
