using System.ComponentModel.DataAnnotations;

namespace backend.Modules.Organization.DTOs;

public class UpdateOrganizationDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    public string? Industry { get; set; }
    
    [Url]
    public string? Website { get; set; }
    
    public string? Address { get; set; }
    public string? TimeZone { get; set; }
    public string? Language { get; set; }
    public string? Currency { get; set; }
}
