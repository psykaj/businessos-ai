using System.ComponentModel.DataAnnotations;
using backend.Common;
using backend.Entities;

namespace backend.Modules.Scorecards.Entities;

public class Scorecard : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    public backend.Entities.Organization? Organization { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Department { get; set; } = "Company-Wide";

    public DateTime PeriodStart { get; set; }
    
    public DateTime PeriodEnd { get; set; }

    [Range(0, 100)]
    public decimal OverallScore { get; set; } = 0;
    
    public string KpiSummaryJson { get; set; } = "[]"; // Serialized summary of KPIs included in this scorecard
}
