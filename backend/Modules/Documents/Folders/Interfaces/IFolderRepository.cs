using backend.Modules.Documents.Entities;

namespace backend.Modules.Documents.Folders.Interfaces;

public interface IFolderRepository
{
    Task<Folder?> GetByIdAsync(Guid organizationId, Guid id);
    Task<List<Folder>> GetByParentIdAsync(Guid organizationId, Guid? parentFolderId);
    Task<List<Folder>> GetAllAsync(Guid organizationId);
    Task AddAsync(Folder folder);
    Task UpdateAsync(Folder folder);
    Task DeleteAsync(Folder folder);
}
