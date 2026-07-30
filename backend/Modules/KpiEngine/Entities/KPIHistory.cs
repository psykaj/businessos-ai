using System.ComponentModel.DataAnnotations;
using backend.Common;
using backend.Entities;

namespace backend.Modules.KpiEngine.Entities;

public class KPIHistory : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid KpiId { get; set; }

    public KPI? KPI { get; set; }

    public decimal Value { get; set; }

    public DateTime PeriodStart { get; set; }

    public DateTime PeriodEnd { get; set; }

    public string Notes { get; set; } = string.Empty;
}
