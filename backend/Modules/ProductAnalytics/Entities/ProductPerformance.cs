using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.ProductAnalytics.Entities;

public class ProductPerformance : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public backend.Entities.Organization? Organization { get; set; }

    public Guid ProductId { get; set; }

    [MaxLength(50)]
    public string ProductSku { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Period { get; set; } = string.Empty;

    public int UnitsSold { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RevenueGenerated { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitCost { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalProfit { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GrossMarginPercentage { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ReturnRatePercentage { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal InventoryTurnoverRate { get; set; }

    public bool IsTopPerformer { get; set; }

    public bool IsLeastPerformer { get; set; }

    public string? RecommendationNotes { get; set; }
}
