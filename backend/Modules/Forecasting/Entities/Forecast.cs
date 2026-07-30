using System.ComponentModel.DataAnnotations;
using backend.Common;
using backend.Entities;

namespace backend.Modules.Forecasting.Entities;

public class Forecast : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    public backend.Entities.Organization? Organization { get; set; }

    [Required]
    [MaxLength(100)]
    public string MetricName { get; set; } = string.Empty; // Revenue, Expenses, CashFlow, Demand

    public decimal PredictedValue { get; set; }

    public decimal LowerBound { get; set; }

    public decimal UpperBound { get; set; }

    [Range(0, 100)]
    public decimal ConfidenceLevel { get; set; } = 80;

    public DateTime ForecastDate { get; set; }

    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string ModelUsed { get; set; } = "ARIMA"; // Linear, Prophet, Custom
    
    public string Factors { get; set; } = string.Empty; // JSON of factors influencing this
}
