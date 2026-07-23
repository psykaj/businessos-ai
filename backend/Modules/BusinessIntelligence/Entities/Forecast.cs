using backend.Common;

namespace backend.Modules.BusinessIntelligence.Entities;

public class Forecast : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public string ForecastType { get; set; } = string.Empty; // "Revenue", "Sales", "Lead", "CustomerGrowth", "Subscription"
    public decimal PredictedValue { get; set; }
    public double ConfidenceScore { get; set; } // e.g. 0.85 (85%)
    public DateTime ForecastDate { get; set; }
}
