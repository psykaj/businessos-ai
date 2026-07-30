using System.ComponentModel.DataAnnotations;
using backend.Common;
using backend.Entities;
using backend.Modules.KpiEngine.Entities;

namespace backend.Modules.BusinessGoals.Entities;

public class BusinessGoal : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    public backend.Entities.Organization? Organization { get; set; }

    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Department { get; set; } = "Company-Wide";

    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    [Required]
    public Guid KpiId { get; set; }
    public KPI? KPI { get; set; }

    public decimal TargetValue { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "OnTrack"; // OnTrack, AtRisk, Behind, Achieved

    [Range(0, 100)]
    public decimal ProgressPercentage { get; set; } = 0;
}
