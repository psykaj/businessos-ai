using backend.Modules.Documents.Documents.DTOs;
using backend.Modules.Documents.Entities;

namespace backend.Modules.Documents.Documents.Interfaces;

public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(Guid organizationId, Guid id, bool includeDeleted = false);
    Task<List<Document>> GetByFolderIdAsync(Guid organizationId, Guid? folderId);
    Task<List<Document>> GetFavoritesAsync(Guid organizationId);
    Task<(List<Document> Items, int TotalCount)> SearchAsync(Guid organizationId, DocumentSearchQueryDto query);
    Task AddAsync(Document document);
    Task UpdateAsync(Document document);
    Task AddVersionAsync(DocumentVersion version);
    Task<DocumentVersion?> GetVersionByIdAsync(Guid organizationId, Guid versionId);
    Task<List<DocumentVersion>> GetVersionsByDocumentIdAsync(Guid organizationId, Guid documentId);
}
