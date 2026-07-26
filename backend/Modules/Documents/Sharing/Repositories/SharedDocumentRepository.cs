using backend.Modules.Documents.Entities;
using backend.Modules.Documents.Sharing.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Documents.Sharing.Repositories;

public class SharedDocumentRepository : ISharedDocumentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SharedDocumentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SharedDocument?> GetByIdAsync(Guid organizationId, Guid id)
    {
        return await _dbContext.SharedDocuments
            .Include(s => s.Document)
            .FirstOrDefaultAsync(s => s.OrganizationId == organizationId && s.Id == id && !s.IsDeleted);
    }

    public async Task<SharedDocument?> GetByTokenAsync(string publicToken)
    {
        return await _dbContext.SharedDocuments
            .Include(s => s.Document)
            .FirstOrDefaultAsync(s => s.PublicShareToken == publicToken && s.IsActive && !s.IsDeleted);
    }

    public async Task<List<SharedDocument>> GetByDocumentIdAsync(Guid organizationId, Guid documentId)
    {
        return await _dbContext.SharedDocuments
            .Include(s => s.Document)
            .Where(s => s.OrganizationId == organizationId && s.DocumentId == documentId && !s.IsDeleted)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(SharedDocument sharedDocument)
    {
        await _dbContext.SharedDocuments.AddAsync(sharedDocument);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(SharedDocument sharedDocument)
    {
        _dbContext.SharedDocuments.Update(sharedDocument);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(SharedDocument sharedDocument)
    {
        sharedDocument.IsDeleted = true;
        sharedDocument.DeletedAt = DateTime.UtcNow;
        _dbContext.SharedDocuments.Update(sharedDocument);
        await _dbContext.SaveChangesAsync();
    }
}
