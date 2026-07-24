using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Modules.AiAgent.Entities;

public class Recommendation : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = "General"; // CRM, Sales, Marketing, Invoicing, BI

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Priority { get; set; } = "Medium"; // High, Medium, Low

    [Required]
    public string SuggestedAction { get; set; } = string.Empty;

    public string? Reason { get; set; }

    public bool IsApplied { get; set; } = false;

    public DateTime? AppliedAt { get; set; }
}
