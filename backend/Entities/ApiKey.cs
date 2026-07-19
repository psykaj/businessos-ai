using backend.Common;

namespace backend.Entities;

public class ApiKey : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }

    public string Name { get; set; } = string.Empty;
    public string KeyHash { get; set; } = string.Empty;
    
    public DateTime? LastUsedAt { get; set; }
    public string Status { get; set; } = "Active"; // Active, Revoked
}
