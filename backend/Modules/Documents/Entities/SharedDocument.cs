using backend.Common;

namespace backend.Modules.Documents.Entities;

public class SharedDocument : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public Guid? SharedWithUserId { get; set; }
    public string? SharedWithEmail { get; set; }
    public string AccessLevel { get; set; } = "Read"; // Read, Comment, Edit, Admin
    public string PermissionType { get; set; } = "InternalUser"; // InternalUser, PublicLink
    public string? PublicShareToken { get; set; }
    public string? Passcode { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public int AccessCount { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid SharedById { get; set; }
}
