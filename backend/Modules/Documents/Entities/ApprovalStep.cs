using backend.Common;

namespace backend.Modules.Documents.Entities;

public class ApprovalStep : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid ApprovalRequestId { get; set; }
    public ApprovalRequest? ApprovalRequest { get; set; }

    public int Sequence { get; set; }
    public Guid? ApproverId { get; set; }
    public string ApproverEmail { get; set; } = string.Empty;
    public string? ApproverName { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Skipped
    public string? Comments { get; set; }
    public DateTime? ActionDate { get; set; }
    public bool IsParallelGroup { get; set; }
}
