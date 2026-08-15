using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;
using backend.Entities;

namespace backend.Modules.BusinessGoals.Entities;

public class BusinessGoal : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    public backend.Entities.Organization? Organization { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string GoalType { get; set; } = "Custom"; // Revenue, Profit, Sales, Customers, etc.

    public decimal TargetValue { get; set; }

    public decimal CurrentValue { get; set; }

    [MaxLength(20)]
    public string Unit { get; set; } = string.Empty;

    [MaxLength(10)]
    public string Currency { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    
    public DateTime TargetDate { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "NotStarted"; // NotStarted, OnTrack, AtRisk, Behind, Completed, Expired, Paused

    [MaxLength(20)]
    public string Priority { get; set; } = "Medium";

    public Guid? OwnerUserId { get; set; }

    public DateTime? CompletedAt { get; set; }

    public bool IsActive { get; set; } = true;

    [Column(TypeName = "jsonb")]
    public string Metadata { get; set; } = "{}";

    [Range(0, 100)]
    public decimal ProgressPercentage { get; set; } = 0;

    public ICollection<BusinessKPI> KPIs { get; set; } = new List<BusinessKPI>();
}
