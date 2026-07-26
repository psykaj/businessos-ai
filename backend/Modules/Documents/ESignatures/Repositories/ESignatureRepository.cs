using backend.Modules.Documents.ESignatures.Interfaces;
using backend.Modules.Documents.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Documents.ESignatures.Repositories;

public class ESignatureRepository : IESignatureRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ESignatureRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SignatureRequest?> GetByIdAsync(Guid organizationId, Guid id)
    {
        return await _dbContext.SignatureRequests
            .Include(r => r.Document)
            .Include(r => r.Recipients)
            .FirstOrDefaultAsync(r => r.OrganizationId == organizationId && r.Id == id && !r.IsDeleted);
    }

    public async Task<SignatureRecipient?> GetRecipientByTokenAsync(string securityToken)
    {
        return await _dbContext.SignatureRecipients
            .Include(r => r.SignatureRequest)
                .ThenInclude(sr => sr!.Document)
            .FirstOrDefaultAsync(r => r.SecurityToken == securityToken && !r.IsDeleted);
    }

    public async Task<List<SignatureRequest>> GetByDocumentIdAsync(Guid organizationId, Guid documentId)
    {
        return await _dbContext.SignatureRequests
            .Include(r => r.Document)
            .Include(r => r.Recipients)
            .Where(r => r.OrganizationId == organizationId && r.DocumentId == documentId && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<SignatureRequest>> GetExpiredRequestsAsync(Guid organizationId)
    {
        var now = DateTime.UtcNow;
        return await _dbContext.SignatureRequests
            .Where(r => r.OrganizationId == organizationId && r.Status == "Pending" && r.ExpiresAt != null && r.ExpiresAt < now && !r.IsDeleted)
            .ToListAsync();
    }

    public async Task AddAsync(SignatureRequest request)
    {
        await _dbContext.SignatureRequests.AddAsync(request);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(SignatureRequest request)
    {
        _dbContext.SignatureRequests.Update(request);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateRecipientAsync(SignatureRecipient recipient)
    {
        _dbContext.SignatureRecipients.Update(recipient);
        await _dbContext.SaveChangesAsync();
    }
}
