using backend.Common;

namespace backend.Modules.Documents.Entities;

public class Document : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid? FolderId { get; set; }
    public Folder? Folder { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string MimeType { get; set; } = "application/octet-stream";
    public string FileExtension { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string StorageProvider { get; set; } = "LocalStorage";

    public string Status { get; set; } = "Active"; // Active, Draft, InReview, PendingSignature, Signed, Archived
    public Guid OwnerId { get; set; }
    public Guid? CurrentVersionId { get; set; }
    public int VersionCount { get; set; } = 1;
    public bool IsFavorite { get; set; }

    public string? Tags { get; set; } // JSON array of tag strings
    public string? Metadata { get; set; } // JSON metadata key-value pairs

    public ICollection<DocumentVersion> Versions { get; set; } = new List<DocumentVersion>();
    public ICollection<ApprovalRequest> ApprovalRequests { get; set; } = new List<ApprovalRequest>();
    public ICollection<SignatureRequest> SignatureRequests { get; set; } = new List<SignatureRequest>();
    public ICollection<SharedDocument> Shares { get; set; } = new List<SharedDocument>();
}
