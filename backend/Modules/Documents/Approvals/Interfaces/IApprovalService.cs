using backend.Modules.Documents.Approvals.DTOs;

namespace backend.Modules.Documents.Approvals.Interfaces;

public interface IApprovalService
{
    Task<ApprovalRequestDto> CreateAsync(Guid organizationId, Guid requestedById, string requestedByName, CreateApprovalRequestDto dto);
    Task<ApprovalRequestDto> GetByIdAsync(Guid organizationId, Guid id);
    Task<List<ApprovalRequestDto>> GetByDocumentIdAsync(Guid organizationId, Guid documentId);
    Task<List<ApprovalRequestDto>> GetPendingForUserAsync(Guid organizationId, Guid userId, string userEmail);
    Task<ApprovalRequestDto> ApproveStepAsync(Guid organizationId, Guid approvalRequestId, Guid userId, string userEmail, ApproveRejectStepDto dto);
    Task<ApprovalRequestDto> RejectStepAsync(Guid organizationId, Guid approvalRequestId, Guid userId, string userEmail, ApproveRejectStepDto dto);
    Task<ApprovalRequestDto> EscalateAsync(Guid organizationId, Guid approvalRequestId, EscalateApprovalDto dto);
    Task ProcessOverdueEscalationsAsync(Guid organizationId);
}
