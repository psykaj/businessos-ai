using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.MarketingROI.Entities;

public class MarketingPerformance : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public backend.Entities.Organization? Organization { get; set; }

    public Guid? CampaignId { get; set; }

    [Required]
    [MaxLength(200)]
    public string CampaignName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Channel { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Period { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Spend { get; set; }

    public int Impressions { get; set; }

    public int Clicks { get; set; }

    public int Conversions { get; set; }

    public int CustomersAcquired { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CustomerAcquisitionCost { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RevenueGenerated { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ReturnOnAdSpend { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal MarketingRoiPercentage { get; set; }

    public bool IsProfitable { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalSpend { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RevenueAttributed { get; set; }

    public int LeadsGenerated { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ConversionRatePercentage { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Recommendation { get; set; } = string.Empty;
}
