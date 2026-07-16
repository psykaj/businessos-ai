using backend.Common;

namespace backend.Entities;

public class Organization : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Industry { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? LogoUrl { get; set; }
    public string? Website { get; set; }
    
    // Address info
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    
    // Preferences
    public string? TimeZone { get; set; }
    public string? Currency { get; set; }
    
    // Subscription
    public string? SubscriptionPlan { get; set; }
    public string? SubscriptionStatus { get; set; }
    
    public bool IsActive { get; set; } = true;

    // Navigation property
    public ICollection<User> Users { get; set; } = new List<User>();
}
