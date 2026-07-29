using backend.Common;

namespace backend.Entities;

public class ApiKey : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }

    public string Name { get; set; } = string.Empty;
    public string KeyHash { get; set; } = string.Empty;
    
    public string Scopes { get; set; } = string.Empty; // Comma-separated or JSON list of scopes
    
    public DateTime? ExpiresAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public DateTime? RotatedAt { get; set; }
    
    public string Status { get; set; } = "Active"; // Active, Revoked, Expired
    
    public Guid? CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }
}
