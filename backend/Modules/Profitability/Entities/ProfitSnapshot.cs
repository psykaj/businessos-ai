using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.Profitability.Entities;

public class ProfitSnapshot : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public backend.Entities.Organization? Organization { get; set; }

    [Required]
    public DateTime SnapshotDate { get; set; }

    [Required]
    [MaxLength(30)]
    public string Period { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal GrossRevenue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CostOfGoodsSold { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GrossProfit { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GrossMarginPercentage { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal OperatingExpenses { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal NetProfit { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal NetMarginPercentage { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Ebitda { get; set; }

    public string? ProfitCenterBreakdownJson { get; set; }
}
