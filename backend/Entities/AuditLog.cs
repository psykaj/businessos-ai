using backend.Common;

namespace backend.Entities;

public class AuditLog : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }

    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public string Action { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public string? IPAddress { get; set; }
    public string? UserAgent { get; set; }
}
