using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.CustomerAnalytics.Entities;

public class CustomerPerformance : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public backend.Entities.Organization? Organization { get; set; }

    public Guid CustomerId { get; set; }

    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string CustomerSegment { get; set; } = "Standard";

    [Column(TypeName = "decimal(18,2)")]
    public decimal LifetimeValue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal AcquisitionCost { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal LtvToCacRatio { get; set; }

    public int TotalOrders { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal AverageOrderValue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RepeatPurchaseRate { get; set; }

    public DateTime FirstPurchaseDate { get; set; } = DateTime.UtcNow;

    public DateTime LastPurchaseDate { get; set; } = DateTime.UtcNow;

    public int DaysInactive { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Range(0, 100)]
    public decimal ChurnRiskScore { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Active";
}
