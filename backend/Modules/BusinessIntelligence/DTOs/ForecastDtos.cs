namespace backend.Modules.BusinessIntelligence.DTOs;

public class ForecastDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string ForecastType { get; set; } = string.Empty;
    public decimal PredictedValue { get; set; }
    public double ConfidenceScore { get; set; }
    public DateTime ForecastDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GenerateForecastRequestDto
{
    public string ForecastType { get; set; } = string.Empty; // "Revenue", "Sales", "Lead", "CustomerGrowth", "Subscription"
    public int HorizonDays { get; set; } = 30; // 30, 60, 90 days ahead
}

public class ForecastSummaryDto
{
    public string ForecastType { get; set; } = string.Empty;
    public decimal TotalPredicted { get; set; }
    public decimal GrowthRate { get; set; }
    public double AverageConfidence { get; set; }
    public List<ForecastDto> DataPoints { get; set; } = new();
}
