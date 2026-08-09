using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Entities;

namespace backend.Modules.BusinessIntelligence.Interfaces;

public interface IBusinessInsightAggregator
{
    Task<List<BriefingItem>> GatherInsightsAsync(Guid organizationId, DateTime date, CancellationToken cancellationToken = default);
}

public interface IInsightPrioritizer
{
    List<BriefingItem> Prioritize(List<BriefingItem> items, int maxItems = 15);
}

public interface IBriefingGenerator
{
    Task<BusinessBriefing> GenerateBriefingAsync(Guid organizationId, DateTime date, CancellationToken cancellationToken = default);
}

public interface IBusinessBriefingService
{
    Task<BusinessBriefingDto?> GetTodayBriefingAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<BusinessBriefingDto?> GetBriefingByDateAsync(Guid organizationId, DateTime date, CancellationToken cancellationToken = default);
    Task<BusinessBriefingDto> GenerateBriefingAsync(Guid organizationId, DateTime date, bool forceRegeneration, CancellationToken cancellationToken = default);
}

public interface IProactiveAlertService
{
    Task<List<ProactiveAlertDto>> GetAlertsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<ProactiveAlertDto>> GetUnreadAlertsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<ProactiveAlertDto>> GetActionRequiredAlertsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    
    Task MarkAsReadAsync(Guid alertId, Guid organizationId, CancellationToken cancellationToken = default);
    Task DismissAsync(Guid alertId, Guid organizationId, CancellationToken cancellationToken = default);
    Task ResolveAsync(Guid alertId, Guid organizationId, CancellationToken cancellationToken = default);
    
    // Creates an alert if one with the same deduplication key doesn't exist and isn't unresolved
    Task<ProactiveAlert?> CreateAlertAsync(ProactiveAlert alert, CancellationToken cancellationToken = default);
}

public interface IBusinessBriefingScheduler
{
    Task ProcessScheduledBriefingsAsync(CancellationToken cancellationToken = default);
}
