using backend.Common;

namespace backend.Modules.Documents.Entities;

public class ApprovalRequest : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Cancelled
    public int CurrentStepSequence { get; set; } = 1;
    public bool IsSequential { get; set; } = true;
    public DateTime? DueDate { get; set; }
    public bool AutoEscalate { get; set; }
    public Guid? EscalatedToUserId { get; set; }
    public Guid RequestedById { get; set; }
    public string? RequestedByName { get; set; }

    public ICollection<ApprovalStep> Steps { get; set; } = new List<ApprovalStep>();
}
