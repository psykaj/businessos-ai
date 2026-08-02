using System;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.CommunicationAnalytics.DTOs;

namespace backend.Modules.CommunicationHub.CommunicationAnalytics.Interfaces;

public interface ICommunicationAnalyticsService
{
    Task<AnalyticsOverviewDto> GetOverviewAsync(Guid organizationId);
    Task<bool> SubmitCsatRatingAsync(Guid conversationId, Guid organizationId, SubmitCsatRequest request);
    Task RunDailyAggregationAsync();
}
