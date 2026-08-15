using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Modules.BusinessGoals.Entities;

public class BusinessKPI : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    public backend.Entities.Organization? Organization { get; set; }

    [Required]
    public Guid BusinessGoalId { get; set; }

    public BusinessGoal? BusinessGoal { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string KPIType { get; set; } = string.Empty; // e.g., Revenue, NewCustomers

    [MaxLength(100)]
    public string MetricKey { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal CurrentValue { get; set; } = 0;

    public decimal TargetValue { get; set; } = 0;

    public decimal PreviousValue { get; set; } = 0;

    [MaxLength(20)]
    public string Unit { get; set; } = string.Empty;

    [MaxLength(10)]
    public string Currency { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Direction { get; set; } = "HigherIsBetter"; // HigherIsBetter, LowerIsBetter, TargetRange

    [MaxLength(20)]
    public string Frequency { get; set; } = "Monthly";

    [MaxLength(20)]
    public string Status { get; set; } = "OnTrack";

    public DateTime LastCalculatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;
}
