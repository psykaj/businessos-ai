using backend.Common;

namespace backend.Modules.Documents.Entities;

public class DocumentVersion : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public int VersionNumber { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string StorageProvider { get; set; } = "LocalStorage";
    public long FileSize { get; set; }
    public string MimeType { get; set; } = "application/octet-stream";
    public string? ChangesSummary { get; set; }
    public Guid? UploadedById { get; set; }
    public string? UploadedByName { get; set; }
}
