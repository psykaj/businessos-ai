using System.ComponentModel.DataAnnotations;
using backend.Common;
using backend.Entities;

namespace backend.Modules.Benchmarks.Entities;

public class Benchmark : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    public backend.Entities.Organization? Organization { get; set; }

    [Required]
    [MaxLength(100)]
    public string MetricName { get; set; } = string.Empty; // e.g., Profit Margin

    public decimal IndustryAverage { get; set; }
    
    public decimal TopQuartile { get; set; }

    [MaxLength(100)]
    public string Industry { get; set; } = string.Empty;

    public DateTime ValidFrom { get; set; }
    
    public DateTime ValidTo { get; set; }

    public string Source { get; set; } = string.Empty;
}
