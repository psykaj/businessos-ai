namespace backend.Modules.BusinessIntelligence.DTOs;

public class DashboardFilterDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Period { get; set; } // "7d", "30d", "90d", "1y", "custom"
    public string? ComparisonPeriod { get; set; } // "previous_period", "previous_year", "none"
    public string? CategoryFilter { get; set; }
    public Guid? AssignedToUserId { get; set; }
}

public class DashboardMetricItemDto
{
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public decimal CurrentValue { get; set; }
    public decimal PreviousValue { get; set; }
    public double PercentageChange { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string Trend { get; set; } = string.Empty; // "Up", "Down", "Neutral"
}

public class ChartDataPointDto
{
    public string DateLabel { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Value { get; set; }
    public decimal? ComparisonValue { get; set; }
}

public class ExecutiveDashboardDto
{
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public string PeriodLabel { get; set; } = string.Empty;
    public List<DashboardMetricItemDto> KeyMetrics { get; set; } = new();
    public List<ChartDataPointDto> PrimaryRevenueTrend { get; set; } = new();
    public List<InsightDto> TopInsights { get; set; } = new();
    public List<GoalDto> ActiveGoals { get; set; } = new();
}

public class CEODashboardDto : ExecutiveDashboardDto
{
    public decimal TotalRevenue { get; set; }
    public decimal TotalLeads { get; set; }
    public int ActiveCustomers { get; set; }
    public double OverallHealthScore { get; set; }
    public List<ChartDataPointDto> GrowthVelocity { get; set; } = new();
}

public class SalesDashboardDto : ExecutiveDashboardDto
{
    public decimal TotalPipelineValue { get; set; }
    public decimal WonDealsValue { get; set; }
    public int TotalDealsCount { get; set; }
    public double WinRate { get; set; }
    public decimal AverageDealSize { get; set; }
    public double AverageSalesCycleDays { get; set; }
    public List<ChartDataPointDto> PipelineByStage { get; set; } = new();
    public List<TeamMemberPerformanceDto> RepRankings { get; set; } = new();
}

public class MarketingDashboardDto : ExecutiveDashboardDto
{
    public int TotalLeadsGenerated { get; set; }
    public double LeadConversionRate { get; set; }
    public int TotalCampaigns { get; set; }
    public decimal AverageCampaignROI { get; set; }
    public int TotalQRScans { get; set; }
    public List<ChartDataPointDto> LeadsBySource { get; set; } = new();
    public List<CampaignPerformanceDto> CampaignPerformance { get; set; } = new();
}

public class FinanceDashboardDto : ExecutiveDashboardDto
{
    public decimal TotalRevenue { get; set; }
    public decimal MonthlyRecurringRevenue { get; set; }
    public decimal AnnualRecurringRevenue { get; set; }
    public decimal TotalOutstandingInvoices { get; set; }
    public double ChurnRate { get; set; }
    public List<ChartDataPointDto> MonthlyRevenueBreakdown { get; set; } = new();
}

public class OperationsDashboardDto : ExecutiveDashboardDto
{
    public int ActiveWorkflows { get; set; }
    public int TotalWorkflowExecutions { get; set; }
    public double ExecutionSuccessRate { get; set; }
    public int TotalAIRequests { get; set; }
    public int ConnectedIntegrations { get; set; }
    public List<ChartDataPointDto> ExecutionVolumeTrend { get; set; } = new();
}

public class TeamPerformanceDashboardDto : ExecutiveDashboardDto
{
    public int TotalTeamMembers { get; set; }
    public int TotalActivitiesLogged { get; set; }
    public int TotalDealsClosed { get; set; }
    public decimal TotalRevenueGenerated { get; set; }
    public List<TeamMemberPerformanceDto> MemberDetails { get; set; } = new();
}

public class TeamMemberPerformanceDto
{
    public Guid UserId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int ActivitiesCount { get; set; }
    public int DealsWonCount { get; set; }
    public decimal RevenueGenerated { get; set; }
    public double ConversionRate { get; set; }
}

public class CampaignPerformanceDto
{
    public Guid CampaignId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public int LeadsGenerated { get; set; }
    public decimal RevenueGenerated { get; set; }
    public decimal ROI { get; set; }
}
