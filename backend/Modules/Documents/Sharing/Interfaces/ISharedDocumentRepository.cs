using backend.Modules.Documents.Entities;

namespace backend.Modules.Documents.Sharing.Interfaces;

public interface ISharedDocumentRepository
{
    Task<SharedDocument?> GetByIdAsync(Guid organizationId, Guid id);
    Task<SharedDocument?> GetByTokenAsync(string publicToken);
    Task<List<SharedDocument>> GetByDocumentIdAsync(Guid organizationId, Guid documentId);
    Task AddAsync(SharedDocument sharedDocument);
    Task UpdateAsync(SharedDocument sharedDocument);
    Task DeleteAsync(SharedDocument sharedDocument);
}
