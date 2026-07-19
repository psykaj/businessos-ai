namespace backend.Modules.Organization.DTOs;

public class OrganizationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Industry { get; set; }
    public string? LogoUrl { get; set; }
    public string? Website { get; set; }
    public string? Address { get; set; }
    public string? TimeZone { get; set; }
    public string? Language { get; set; }
    public string? Currency { get; set; }
    public Guid? SubscriptionId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
