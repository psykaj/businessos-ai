using backend.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

public class Brand : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    [ForeignKey("OrganizationId")]
    public Organization? Organization { get; set; }

    [Required]
    [MaxLength(100)]
    public string CompanyName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? LogoUrl { get; set; }

    [MaxLength(500)]
    public string? FaviconUrl { get; set; }

    [MaxLength(7)]
    public string PrimaryColor { get; set; } = "#000000";

    [MaxLength(7)]
    public string SecondaryColor { get; set; } = "#ffffff";

    [MaxLength(7)]
    public string AccentColor { get; set; } = "#0ea5e9";

    [MaxLength(100)]
    public string FontFamily { get; set; } = "Inter, sans-serif";

    [MaxLength(500)]
    public string? FooterText { get; set; }

    [MaxLength(255)]
    [EmailAddress]
    public string? SupportEmail { get; set; }
}
