using System.ComponentModel.DataAnnotations;
using backend.Common;
using backend.Entities;

namespace backend.Modules.KpiEngine.Entities;

public class KPI : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    public backend.Entities.Organization? Organization { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty; // e.g., Financial, Sales, Marketing, Operations

    [Required]
    [MaxLength(20)]
    public string Unit { get; set; } = string.Empty; // e.g., Currency, Percentage, Count

    public decimal CurrentValue { get; set; } = 0;
    
    public decimal TargetValue { get; set; } = 0;

    public decimal PreviousValue { get; set; } = 0;

    [MaxLength(20)]
    public string Frequency { get; set; } = "Monthly"; // Daily, Weekly, Monthly, Quarterly, Yearly

    public bool IsHigherBetter { get; set; } = true;
    
    public DateTime LastCalculatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<KPIHistory> History { get; set; } = new List<KPIHistory>();
}
