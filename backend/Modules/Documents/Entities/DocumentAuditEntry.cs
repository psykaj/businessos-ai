using backend.Common;

namespace backend.Modules.Documents.Entities;

public class DocumentAuditEntry : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid? DocumentId { get; set; }
    public Document? Document { get; set; }

    public string EntityType { get; set; } = "Document"; // Document, Folder, Version, Approval, Signature, Share
    public string Action { get; set; } = "Viewed"; // Uploaded, Downloaded, Viewed, Renamed, Moved, Copied, Deleted, Restored, Shared, VersionCreated, ApprovalRequested, Approved, Rejected, SignatureRequested, Signed, Declined
    public Guid? PerformedById { get; set; }
    public string? PerformedByName { get; set; }
    public string? IpAddress { get; set; }
    public string? DetailsJson { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
