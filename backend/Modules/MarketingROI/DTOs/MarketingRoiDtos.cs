using System;
using System.Collections.Generic;

namespace backend.Modules.MarketingROI.DTOs;

public class MarketingPerformanceDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Channel { get; set; } = string.Empty;
    public string CampaignName { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public decimal TotalSpend { get; set; }
    public int Impressions { get; set; }
    public int Clicks { get; set; }
    public int LeadsGenerated { get; set; }
    public int CustomersAcquired { get; set; }
    public decimal RevenueAttributed { get; set; }
    public decimal CustomerAcquisitionCost { get; set; }
    public decimal ReturnOnAdSpend { get; set; }
    public decimal ConversionRatePercentage { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Recommendation { get; set; }
}

public class CreateMarketingPerformanceRequest
{
    public string Channel { get; set; } = string.Empty;
    public string CampaignName { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public decimal TotalSpend { get; set; }
    public int Impressions { get; set; }
    public int Clicks { get; set; }
    public int LeadsGenerated { get; set; }
    public int CustomersAcquired { get; set; }
    public decimal RevenueAttributed { get; set; }
    public string Status { get; set; } = "Active";
}

public class MarketingRoiSummaryDto
{
    public Guid OrganizationId { get; set; }
    public decimal TotalAdSpend { get; set; }
    public decimal TotalRevenueAttributed { get; set; }
    public decimal OverallRoas { get; set; }
    public int TotalLeadsGenerated { get; set; }
    public int TotalCustomersAcquired { get; set; }
    public decimal AverageCac { get; set; }
    public List<MarketingPerformanceDto> ChannelPerformances { get; set; } = new();
    public List<string> BudgetAllocationRecommendations { get; set; } = new();
}
