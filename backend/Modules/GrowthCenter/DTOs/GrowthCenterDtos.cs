using System;
using System.Collections.Generic;
using backend.Modules.Benchmarking.DTOs;
using backend.Modules.BusinessPerformance.DTOs;
using backend.Modules.CustomerAnalytics.DTOs;
using backend.Modules.GrowthRecommendations.DTOs;
using backend.Modules.MarketingROI.DTOs;
using backend.Modules.ProductAnalytics.DTOs;
using backend.Modules.Profitability.DTOs;
using backend.Modules.RevenueAnalytics.DTOs;

namespace backend.Modules.GrowthCenter.DTOs;

public class GrowthCenterOverviewDto
{
    public Guid OrganizationId { get; set; }
    public decimal GrowthHealthScore { get; set; } // 0 - 100
    public decimal ProjectedAnnualRevenue { get; set; }
    public decimal NetRevenueRetention { get; set; }
    public decimal OverallLtvToCac { get; set; }
    
    // Core breakdowns
    public BusinessPerformanceDashboardDto BusinessPerformance { get; set; } = new();
    public RevenueAnalyticsSummaryDto RevenueSummary { get; set; } = new();
    public ProfitabilityAnalysisDto ProfitabilityAnalysis { get; set; } = new();
    public ProductPerformanceSummaryDto ProductSummary { get; set; } = new();
    public CustomerAnalyticsSummaryDto CustomerSummary { get; set; } = new();
    public MarketingRoiSummaryDto MarketingRoiSummary { get; set; } = new();
    public BenchmarkComparisonSummaryDto BenchmarkSummary { get; set; } = new();
    public GrowthRecommendationSummaryDto AiRecommendations { get; set; } = new();
    
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}

public class ActionPlanDto
{
    public Guid OrganizationId { get; set; }
    public string OverallAssessment { get; set; } = string.Empty;
    public decimal EstimatedTotalProfitGain { get; set; }
    public List<string> RevenueAccelerationActions { get; set; } = new();
    public List<string> CostReductionActions { get; set; } = new();
    public List<string> RiskMitigationActions { get; set; } = new();
    public List<GrowthRecommendationDto> PrioritizedRecommendations { get; set; } = new();
}

public class GrowthCenterRefreshRequest
{
    public bool ForceRecalculation { get; set; } = true;
}
