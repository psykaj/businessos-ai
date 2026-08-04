using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.RevenueAnalytics.Entities;

public class RevenueSnapshot : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public backend.Entities.Organization? Organization { get; set; }

    [Required]
    public DateTime SnapshotDate { get; set; }

    [Required]
    [MaxLength(20)]
    public string PeriodType { get; set; } = "Monthly"; // Daily, Monthly, Quarterly

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalRevenue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal MonthlyRecurringRevenue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal AnnualRecurringRevenue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal NewRevenue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ExpansionRevenue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ContractionRevenue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ChurnedRevenue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal NetNewRevenue { get; set; }

    [MaxLength(10)]
    public string Currency { get; set; } = "USD";

    public string? Notes { get; set; }
}
