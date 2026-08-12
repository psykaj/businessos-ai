using System.ComponentModel.DataAnnotations;
using backend.Modules.Outcomes.Entities;

namespace backend.Modules.Outcomes.DTOs;

public class BusinessOutcomeDto
{
    public Guid Id { get; set; }
    public Guid BusinessId { get; set; }
    public string OutcomeType { get; set; } = string.Empty;
    public string SourceType { get; set; } = string.Empty;
    public string SourceId { get; set; } = string.Empty;
    public string? RelatedEntityType { get; set; }
    public string? RelatedEntityId { get; set; }
    public decimal? BeforeValue { get; set; }
    public decimal? AfterValue { get; set; }
    public decimal? ChangeValue { get; set; }
    public decimal? ChangePercentage { get; set; }
    public string? Currency { get; set; }
    public int? TimeSavedMinutes { get; set; }
    public decimal? RevenueImpact { get; set; }
    public decimal? CostImpact { get; set; }
    public int? CustomerImpact { get; set; }
    public string Confidence { get; set; } = string.Empty;
    public string AttributionLevel { get; set; } = string.Empty;
    public string? MeasurementMethod { get; set; }
    public DateTime OccurredAt { get; set; }
    public DateTime RecordedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Metadata { get; set; }
    public string? Explanation { get; set; }
}

public class CreateOutcomeDto
{
    [Required]
    public string OutcomeType { get; set; } = string.Empty;
    
    [Required]
    public string SourceType { get; set; } = string.Empty;
    
    [Required]
    public string SourceId { get; set; } = string.Empty;
    
    public string? RelatedEntityType { get; set; }
    public string? RelatedEntityId { get; set; }
    
    public decimal? BeforeValue { get; set; }
    public decimal? AfterValue { get; set; }
    public decimal? ChangeValue { get; set; }
    public decimal? ChangePercentage { get; set; }
    
    public string? Currency { get; set; }
    
    public int? TimeSavedMinutes { get; set; }
    public decimal? RevenueImpact { get; set; }
    public decimal? CostImpact { get; set; }
    public int? CustomerImpact { get; set; }
    
    [Required]
    public string Confidence { get; set; } = string.Empty;
    
    public string? MeasurementMethod { get; set; }
    public string? Metadata { get; set; }
    public string? Explanation { get; set; }
    public DateTime? OccurredAt { get; set; }
}

public class RoiSummaryDto
{
    public decimal RevenueGenerated { get; set; }
    public decimal RevenueRecovered { get; set; }
    public decimal CostSaved { get; set; }
    public int TimeSavedMinutes { get; set; }
    public decimal EstimatedTimeValue { get; set; }
    public int CustomersRetained { get; set; }
    public int CustomersConverted { get; set; }
    public int LeadsConverted { get; set; }
    public int SuccessfulActions { get; set; }
    public int SuccessfulAutomations { get; set; }
    
    public Dictionary<string, int> ConfidenceSummary { get; set; } = new();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
