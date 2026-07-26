using backend.Modules.Documents.Entities;
using backend.Modules.Documents.Folders.DTOs;
using backend.Modules.Documents.Folders.Interfaces;

namespace backend.Modules.Documents.Folders.Services;

public class FolderService : IFolderService
{
    private readonly IFolderRepository _repository;

    public FolderService(IFolderRepository repository)
    {
        _repository = repository;
    }

    public async Task<FolderDto> CreateAsync(Guid organizationId, CreateFolderDto dto)
    {
        string parentPath = "/";
        if (dto.ParentFolderId.HasValue)
        {
            var parent = await _repository.GetByIdAsync(organizationId, dto.ParentFolderId.Value)
                ?? throw new KeyNotFoundException($"Parent folder with ID {dto.ParentFolderId} not found.");
            parentPath = parent.Path.EndsWith('/') ? $"{parent.Path}{parent.Name}/" : $"{parent.Path}/{parent.Name}/";
        }

        var folder = new Folder
        {
            OrganizationId = organizationId,
            ParentFolderId = dto.ParentFolderId,
            Name = dto.Name.Trim(),
            Description = dto.Description,
            Path = parentPath,
            Color = dto.Color,
            Icon = dto.Icon
        };

        await _repository.AddAsync(folder);
        return MapToDto(folder);
    }

    public async Task<FolderDto> GetByIdAsync(Guid organizationId, Guid id)
    {
        var folder = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Folder with ID {id} not found.");
        return MapToDto(folder);
    }

    public async Task<List<FolderDto>> GetByParentAsync(Guid organizationId, Guid? parentFolderId)
    {
        var folders = await _repository.GetByParentIdAsync(organizationId, parentFolderId);
        return folders.Select(MapToDto).ToList();
    }

    public async Task<List<FolderTreeDto>> GetTreeAsync(Guid organizationId)
    {
        var allFolders = await _repository.GetAllAsync(organizationId);
        var rootFolders = allFolders.Where(f => f.ParentFolderId == null).ToList();

        List<FolderTreeDto> BuildTree(List<Folder> nodes)
        {
            return nodes.Select(f => new FolderTreeDto(
                f.Id,
                f.OrganizationId,
                f.ParentFolderId,
                f.Name,
                f.Path,
                f.Color,
                f.Icon,
                BuildTree(allFolders.Where(c => c.ParentFolderId == f.Id).ToList())
            )).ToList();
        }

        return BuildTree(rootFolders);
    }

    public async Task<FolderDto> UpdateAsync(Guid organizationId, Guid id, UpdateFolderDto dto)
    {
        var folder = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Folder with ID {id} not found.");

        folder.Name = dto.Name.Trim();
        folder.Description = dto.Description;
        folder.Color = dto.Color;
        folder.Icon = dto.Icon;

        await _repository.UpdateAsync(folder);
        return MapToDto(folder);
    }

    public async Task<FolderDto> MoveAsync(Guid organizationId, Guid id, MoveFolderDto dto)
    {
        var folder = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Folder with ID {id} not found.");

        if (dto.TargetParentFolderId == id)
        {
            throw new InvalidOperationException("Cannot move a folder inside itself.");
        }

        string newPath = "/";
        if (dto.TargetParentFolderId.HasValue)
        {
            var targetParent = await _repository.GetByIdAsync(organizationId, dto.TargetParentFolderId.Value)
                ?? throw new KeyNotFoundException($"Target parent folder {dto.TargetParentFolderId} not found.");
            newPath = targetParent.Path.EndsWith('/') ? $"{targetParent.Path}{targetParent.Name}/" : $"{targetParent.Path}/{targetParent.Name}/";
        }

        folder.ParentFolderId = dto.TargetParentFolderId;
        folder.Path = newPath;

        await _repository.UpdateAsync(folder);
        return MapToDto(folder);
    }

    public async Task DeleteAsync(Guid organizationId, Guid id)
    {
        var folder = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Folder with ID {id} not found.");

        await _repository.DeleteAsync(folder);
    }

    private static FolderDto MapToDto(Folder folder) => new(
        folder.Id,
        folder.OrganizationId,
        folder.ParentFolderId,
        folder.Name,
        folder.Description,
        folder.Path,
        folder.Color,
        folder.Icon,
        folder.SubFolders?.Count(sf => !sf.IsDeleted) ?? 0,
        folder.Documents?.Count(d => !d.IsDeleted) ?? 0,
        folder.CreatedAt,
        folder.UpdatedAt
    );
}
