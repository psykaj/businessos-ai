using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.DailyOperatingLoop.DTOs;

namespace backend.Modules.DailyOperatingLoop.Interfaces;

public interface IDailyBriefingService
{
    Task<DailyBusinessBriefingDto?> GetTodayBriefingAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<DailyBusinessBriefingDto?> GetBriefingByDateAsync(Guid organizationId, DateTime date, CancellationToken cancellationToken = default);
    Task<DailyBusinessBriefingDto> GenerateBriefingAsync(Guid organizationId, DateTime date, bool forceRegeneration = false, CancellationToken cancellationToken = default);
    Task<DailyBusinessBriefingDto> RefreshBriefingAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IDailyPriorityService
{
    Task<DailyPriorityDto?> GetPriorityAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> CompletePriorityAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> DismissPriorityAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> SnoozePriorityAsync(Guid id, DateTime? snoozeUntil = null, CancellationToken cancellationToken = default);
}
