using backend.Modules.Documents.Approvals.Interfaces;
using backend.Modules.Documents.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Documents.Approvals.Repositories;

public class ApprovalRepository : IApprovalRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ApprovalRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApprovalRequest?> GetByIdAsync(Guid organizationId, Guid id)
    {
        return await _dbContext.ApprovalRequests
            .Include(a => a.Document)
            .Include(a => a.Steps)
            .FirstOrDefaultAsync(a => a.OrganizationId == organizationId && a.Id == id && !a.IsDeleted);
    }

    public async Task<List<ApprovalRequest>> GetByDocumentIdAsync(Guid organizationId, Guid documentId)
    {
        return await _dbContext.ApprovalRequests
            .Include(a => a.Document)
            .Include(a => a.Steps)
            .Where(a => a.OrganizationId == organizationId && a.DocumentId == documentId && !a.IsDeleted)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<ApprovalRequest>> GetPendingForUserAsync(Guid organizationId, Guid userId, string userEmail)
    {
        return await _dbContext.ApprovalRequests
            .Include(a => a.Document)
            .Include(a => a.Steps)
            .Where(a => a.OrganizationId == organizationId && a.Status == "Pending" && !a.IsDeleted)
            .Where(a => a.Steps.Any(s => s.Status == "Pending" && (s.ApproverId == userId || s.ApproverEmail.ToLower() == userEmail.ToLower())))
            .OrderBy(a => a.DueDate)
            .ToListAsync();
    }

    public async Task AddAsync(ApprovalRequest request)
    {
        await _dbContext.ApprovalRequests.AddAsync(request);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(ApprovalRequest request)
    {
        _dbContext.ApprovalRequests.Update(request);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<ApprovalRequest>> GetOverdueRequestsAsync(Guid organizationId)
    {
        var now = DateTime.UtcNow;
        return await _dbContext.ApprovalRequests
            .Include(a => a.Steps)
            .Where(a => a.OrganizationId == organizationId && a.Status == "Pending" && a.DueDate != null && a.DueDate < now && !a.IsDeleted)
            .ToListAsync();
    }
}
