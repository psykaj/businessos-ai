using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Entities; // Needed for IBaseEntity if it exists, let's assume standard properties

namespace backend.Modules.Outcomes.Entities;

public class BusinessOutcome 
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid BusinessId { get; set; } // Identifies the tenant

    [Required]
    public OutcomeType OutcomeType { get; set; }

    [Required]
    public OutcomeSourceType SourceType { get; set; }

    [Required]
    [MaxLength(255)]
    public string SourceId { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? RelatedEntityType { get; set; } // e.g. "Customer", "Invoice"

    [MaxLength(255)]
    public string? RelatedEntityId { get; set; }

    // Metrics tracking
    public decimal? BeforeValue { get; set; }
    public decimal? AfterValue { get; set; }
    public decimal? ChangeValue { get; set; }
    public decimal? ChangePercentage { get; set; }

    [MaxLength(10)]
    public string? Currency { get; set; }

    public int? TimeSavedMinutes { get; set; }
    public decimal? RevenueImpact { get; set; }
    public decimal? CostImpact { get; set; }
    public int? CustomerImpact { get; set; } // E.g., count of customers affected

    [Required]
    public OutcomeConfidence Confidence { get; set; }
    
    [Required]
    public AttributionLevel AttributionLevel { get; set; }

    [MaxLength(255)]
    public string? MeasurementMethod { get; set; } // E.g., "Invoice payment after action"

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public OutcomeStatus Status { get; set; } = OutcomeStatus.Active;

    public string? Metadata { get; set; } // JSON blob for any extra data
    
    public string? Explanation { get; set; } // Natural language explanation of the outcome
    
    // Audit fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
}
