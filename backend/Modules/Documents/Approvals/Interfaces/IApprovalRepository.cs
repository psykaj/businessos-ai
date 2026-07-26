using backend.Modules.Documents.Entities;

namespace backend.Modules.Documents.Approvals.Interfaces;

public interface IApprovalRepository
{
    Task<ApprovalRequest?> GetByIdAsync(Guid organizationId, Guid id);
    Task<List<ApprovalRequest>> GetByDocumentIdAsync(Guid organizationId, Guid documentId);
    Task<List<ApprovalRequest>> GetPendingForUserAsync(Guid organizationId, Guid userId, string userEmail);
    Task AddAsync(ApprovalRequest request);
    Task UpdateAsync(ApprovalRequest request);
    Task<List<ApprovalRequest>> GetOverdueRequestsAsync(Guid organizationId);
}
