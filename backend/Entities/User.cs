using backend.Common;

namespace backend.Entities;

public sealed class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public bool IsActive { get; set; } = true;
    
    // Multi-tenant Reference
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }
}
