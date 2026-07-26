using backend.Common;

namespace backend.Modules.Documents.Entities;

public class SignatureRequest : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Message { get; set; }
    public string Status { get; set; } = "Draft"; // Draft, Pending, Completed, Declined, Expired, Cancelled
    public DateTime? ExpiresAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid CreatedById { get; set; }
    public string? CreatedByName { get; set; }
    public string SecurityHash { get; set; } = string.Empty; // Cryptographic SHA256 checksum
    public string? SignatureCertificateUrl { get; set; }

    public ICollection<SignatureRecipient> Recipients { get; set; } = new List<SignatureRecipient>();
}
