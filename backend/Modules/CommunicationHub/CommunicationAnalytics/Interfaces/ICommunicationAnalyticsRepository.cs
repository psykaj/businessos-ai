using System;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.CommunicationAnalytics.DTOs;

namespace backend.Modules.CommunicationHub.CommunicationAnalytics.Interfaces;

public interface ICommunicationAnalyticsRepository
{
    Task<AnalyticsOverviewDto> GetOverviewAsync(Guid organizationId);
    Task<bool> RecordCsatAsync(Guid conversationId, Guid organizationId, int rating, string? feedback);
    Task PerformDailyAggregationAsync();
}
