using backend.Common;

namespace backend.Modules.Documents.Entities;

public class SignatureRecipient : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid SignatureRequestId { get; set; }
    public SignatureRequest? SignatureRequest { get; set; }

    public string SignerName { get; set; } = string.Empty;
    public string SignerEmail { get; set; } = string.Empty;
    public Guid? SignerUserId { get; set; }
    public string Role { get; set; } = "Signer"; // Signer, Viewer, Approver, CC
    public int SigningOrder { get; set; } = 1;
    public string Status { get; set; } = "Pending"; // Pending, Sent, Viewed, Signed, Declined
    public DateTime? ViewedAt { get; set; }
    public DateTime? SignedAt { get; set; }
    public string? SignatureData { get; set; } // Base64 PNG signature or vector SVG
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string SecurityToken { get; set; } = Guid.NewGuid().ToString("N");
    public string? AccessCode { get; set; } // Optional PIN access code
}
