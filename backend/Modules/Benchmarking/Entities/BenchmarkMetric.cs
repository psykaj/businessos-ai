using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.Benchmarking.Entities;

public class BenchmarkMetric : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public backend.Entities.Organization? Organization { get; set; }

    [Required]
    [MaxLength(100)]
    public string MetricName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ComparisonType { get; set; } = string.Empty; // MonthOverMonth, QuarterOverQuarter, YearOverYear, ProductCategory, CustomerSegment, MarketingChannel

    [MaxLength(100)]
    public string DimensionName { get; set; } = "Total";

    [Required]
    [MaxLength(30)]
    public string CurrentPeriod { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal CurrentValue { get; set; }

    [MaxLength(30)]
    public string PreviousPeriod { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PreviousValue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PercentageVariance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? IndustryAverage { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "OnTrack"; // Outperforming, OnTrack, Underperforming

    [Column(TypeName = "decimal(18,2)")]
    public decimal OrganizationValue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal BenchmarkMedian { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal BenchmarkTopQuartile { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PercentileRank { get; set; }
}
