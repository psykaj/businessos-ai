using backend.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

public class Theme : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    [ForeignKey("OrganizationId")]
    public Organization? Organization { get; set; }

    [Required]
    [MaxLength(100)]
    public string ThemeName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string ThemeMode { get; set; } = "Light"; // Light, Dark, System

    [Required]
    public string ThemeJson { get; set; } = "{}";

    public bool IsDefault { get; set; }
}
