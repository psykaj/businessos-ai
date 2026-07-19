namespace backend.Modules.Branding.DTOs;

public class BrandDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? FaviconUrl { get; set; }
    public string PrimaryColor { get; set; } = string.Empty;
    public string SecondaryColor { get; set; } = string.Empty;
    public string AccentColor { get; set; } = string.Empty;
    public string FontFamily { get; set; } = string.Empty;
    public string? FooterText { get; set; }
    public string? SupportEmail { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UpdateBrandDto
{
    public string? CompanyName { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? AccentColor { get; set; }
    public string? FontFamily { get; set; }
    public string? FooterText { get; set; }
    public string? SupportEmail { get; set; }
}
