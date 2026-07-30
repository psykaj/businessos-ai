using System.ComponentModel.DataAnnotations;
using backend.Common;
using backend.Entities;

namespace backend.Modules.AiRecommendations.Entities;

public class AiRecommendation : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    public backend.Entities.Organization? Organization { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Category { get; set; } = "Strategic"; // Strategic, Operational, Financial

    public decimal EstimatedImpact { get; set; }

    [Range(0, 100)]
    public decimal ConfidenceLevel { get; set; } = 90;
    
    public string SuggestedAction { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string Priority { get; set; } = "Medium";

    public bool IsApplied { get; set; } = false;
    
    public DateTime? AppliedAt { get; set; }
}
