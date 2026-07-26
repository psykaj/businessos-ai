using backend.Modules.Documents.Entities;
using backend.Modules.Documents.Folders.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Documents.Folders.Repositories;

public class FolderRepository : IFolderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FolderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Folder?> GetByIdAsync(Guid organizationId, Guid id)
    {
        return await _dbContext.Folders
            .Include(f => f.SubFolders.Where(sf => !sf.IsDeleted))
            .Include(f => f.Documents.Where(d => !d.IsDeleted))
            .FirstOrDefaultAsync(f => f.OrganizationId == organizationId && f.Id == id && !f.IsDeleted);
    }

    public async Task<List<Folder>> GetByParentIdAsync(Guid organizationId, Guid? parentFolderId)
    {
        return await _dbContext.Folders
            .Include(f => f.SubFolders.Where(sf => !sf.IsDeleted))
            .Include(f => f.Documents.Where(d => !d.IsDeleted))
            .Where(f => f.OrganizationId == organizationId && f.ParentFolderId == parentFolderId && !f.IsDeleted)
            .OrderBy(f => f.Name)
            .ToListAsync();
    }

    public async Task<List<Folder>> GetAllAsync(Guid organizationId)
    {
        return await _dbContext.Folders
            .Include(f => f.SubFolders.Where(sf => !sf.IsDeleted))
            .Include(f => f.Documents.Where(d => !d.IsDeleted))
            .Where(f => f.OrganizationId == organizationId && !f.IsDeleted)
            .OrderBy(f => f.Name)
            .ToListAsync();
    }

    public async Task AddAsync(Folder folder)
    {
        await _dbContext.Folders.AddAsync(folder);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Folder folder)
    {
        _dbContext.Folders.Update(folder);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Folder folder)
    {
        folder.IsDeleted = true;
        folder.DeletedAt = DateTime.UtcNow;
        _dbContext.Folders.Update(folder);
        await _dbContext.SaveChangesAsync();
    }
}
