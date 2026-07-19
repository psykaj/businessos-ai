using backend.Common;

namespace backend.Entities;

public class TeamMember : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid RoleId { get; set; }
    public Role? Role { get; set; }

    public string Status { get; set; } = "Active"; // Active, Inactive, Suspended
    public DateTime? LastLogin { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}
