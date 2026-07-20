using backend.Modules.CRM.Enums;

namespace backend.Modules.CRM.DTOs;

public class GlobalSearchResultDto
{
    public string EntityType { get; set; } = string.Empty; // "Lead", "Contact", "Deal", "Company"
    public Guid EntityId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MatchReason { get; set; } = string.Empty;
}

public class CrmOverviewDto
{
    public int TotalLeads { get; set; }
    public int TotalDeals { get; set; }
    public decimal RevenueForecast { get; set; }
    public decimal PipelineValue { get; set; }
    public decimal ConversionRate { get; set; } // Deals won vs Deals total
}

public class SalesPerformanceDto
{
    public DateTime Date { get; set; }
    public decimal Revenue { get; set; }
    public int DealsWon { get; set; }
}
