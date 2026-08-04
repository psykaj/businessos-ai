using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.BusinessPerformance.Entities;

public class BusinessMetric : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public backend.Entities.Organization? Organization { get; set; }

    [Required]
    [MaxLength(100)]
    public string MetricType { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = "Financial";

    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PreviousValue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TargetValue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PercentageChange { get; set; }

    [MaxLength(20)]
    public string Unit { get; set; } = "USD";

    [MaxLength(50)]
    public string Period { get; set; } = string.Empty;

    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;

    public string? MetadataJson { get; set; }
}
