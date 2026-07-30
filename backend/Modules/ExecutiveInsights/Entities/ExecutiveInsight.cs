using System.ComponentModel.DataAnnotations;
using backend.Common;
using backend.Entities;

namespace backend.Modules.ExecutiveInsights.Entities;

public class ExecutiveInsight : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    public backend.Entities.Organization? Organization { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Category { get; set; } = "General"; // Financial, Sales, Operational

    [MaxLength(20)]
    public string Priority { get; set; } = "Medium"; // High, Medium, Low

    public decimal BusinessImpact { get; set; } = 0; // Estimated monetary impact

    [Range(0, 100)]
    public decimal ConfidenceLevel { get; set; } = 80;

    public string SuggestedAction { get; set; } = string.Empty;

    public bool IsRead { get; set; } = false;

    public bool IsActioned { get; set; } = false;

    public Guid? ActionedById { get; set; }
}
