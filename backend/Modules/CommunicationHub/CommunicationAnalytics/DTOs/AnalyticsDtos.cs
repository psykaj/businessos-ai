using System;
using System.Collections.Generic;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.CommunicationAnalytics.DTOs;

public class ChannelUsageDto
{
    public CommunicationChannelType ChannelType { get; set; }
    public int ConversationCount { get; set; }
    public int TotalMessages { get; set; }
    public decimal PercentageOfTotal { get; set; }
}

public class CsatSummaryDto
{
    public decimal AverageRating { get; set; }
    public int TotalResponses { get; set; }
    public int SatisfiedCount { get; set; } // Ratings 4 and 5
    public int NeutralCount { get; set; }   // Rating 3
    public int UnsatisfiedCount { get; set; } // Ratings 1 and 2
}

public class AnalyticsOverviewDto
{
    public Guid OrganizationId { get; set; }
    public int ActiveConversations { get; set; }
    public int TotalMessagesToday { get; set; }
    public decimal AverageResponseTimeMinutes { get; set; }
    public decimal AverageResolutionTimeMinutes { get; set; }
    public List<ChannelUsageDto> ChannelBreakdown { get; set; } = new();
    public CsatSummaryDto CsatSummary { get; set; } = new();
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}

public class SubmitCsatRequest
{
    public int Rating { get; set; } // 1 to 5
    public string? Feedback { get; set; }
}
