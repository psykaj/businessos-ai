using backend.Modules.Documents.Folders.DTOs;

namespace backend.Modules.Documents.Folders.Interfaces;

public interface IFolderService
{
    Task<FolderDto> CreateAsync(Guid organizationId, CreateFolderDto dto);
    Task<FolderDto> GetByIdAsync(Guid organizationId, Guid id);
    Task<List<FolderDto>> GetByParentAsync(Guid organizationId, Guid? parentFolderId);
    Task<List<FolderTreeDto>> GetTreeAsync(Guid organizationId);
    Task<FolderDto> UpdateAsync(Guid organizationId, Guid id, UpdateFolderDto dto);
    Task<FolderDto> MoveAsync(Guid organizationId, Guid id, MoveFolderDto dto);
    Task DeleteAsync(Guid organizationId, Guid id);
}
