using System.ComponentModel.DataAnnotations;
using backend.Common;
using backend.Entities;
using backend.Modules.ExecutiveInsights.Entities;

namespace backend.Modules.DecisionCenter.Entities;

public class DecisionLog : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    public backend.Entities.Organization? Organization { get; set; }

    [Required]
    [MaxLength(150)]
    public string DecisionTitle { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Guid? RelatedInsightId { get; set; }
    
    public ExecutiveInsight? RelatedInsight { get; set; }

    public Guid? DecisionMakerId { get; set; }
    
    [MaxLength(100)]
    public string ExpectedOutcome { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Implemented, Evaluated

    public DateTime DecisionDate { get; set; } = DateTime.UtcNow;
}
