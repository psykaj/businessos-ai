using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.FeedbackAnalytics.DTOs;

namespace backend.Modules.CustomerFeedback.FeedbackAnalytics.Interfaces;

public interface IFeedbackAnalyticsRepository
{
    Task<IEnumerable<SentimentAnalysis>> GetRecentSentimentsAsync(Guid organizationId, int days);
    Task<IEnumerable<Entities.Feedback>> GetRecentFeedbacksAsync(Guid organizationId, int days);
}

public interface IFeedbackAnalyticsService
{
    Task<FeedbackAnalyticsSummaryDto> GetSummaryAsync(FeedbackAnalyticsFilterDto filter);
}
