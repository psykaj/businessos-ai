using System;
using System.Collections.Generic;

namespace backend.Modules.CustomerFeedback.FeedbackAnalytics.DTOs;

public class FeedbackTrendDto
{
    public string PeriodLabel { get; set; } = string.Empty;
    public int TotalFeedbacks { get; set; }
    public int PositiveCount { get; set; }
    public int NeutralCount { get; set; }
    public int NegativeCount { get; set; }
    public decimal AverageRating { get; set; }
}

public class FeedbackAnalyticsSummaryDto
{
    public Guid OrganizationId { get; set; }
    public int TotalVolume { get; set; }
    public int PositiveVolume { get; set; }
    public int NeutralVolume { get; set; }
    public int NegativeVolume { get; set; }
    public decimal PositivePercentage { get; set; }
    public List<string> TopComplaintCategories { get; set; } = new();
    public List<FeedbackTrendDto> MonthlyTrends { get; set; } = new();
}

public class FeedbackAnalyticsFilterDto
{
    public Guid OrganizationId { get; set; }
    public int Months { get; set; } = 6;
}
