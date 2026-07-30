using System;

namespace backend.Modules.Forecasting.DTOs;

public class ForecastDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string MetricName { get; set; } = string.Empty;
    public decimal PredictedValue { get; set; }
    public decimal LowerBound { get; set; }
    public decimal UpperBound { get; set; }
    public decimal ConfidenceLevel { get; set; }
    public DateTime ForecastDate { get; set; }
    public DateTime GeneratedAt { get; set; }
    public string ModelUsed { get; set; } = string.Empty;
    public string Factors { get; set; } = string.Empty;
}
