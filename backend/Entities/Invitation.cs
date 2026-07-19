using backend.Common;

namespace backend.Entities;

public class Invitation : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }

    public Guid RoleId { get; set; }
    public Role? Role { get; set; }

    public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected, Expired
    public DateTime ExpiresAt { get; set; }
}
