using System;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.CommunicationAnalytics.DTOs;
using backend.Modules.CommunicationHub.CommunicationAnalytics.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.CommunicationAnalytics.Services;

public class CommunicationAnalyticsService : ICommunicationAnalyticsService
{
    private readonly ICommunicationAnalyticsRepository _repository;
    private readonly IDistributedCache _cache;
    private readonly ILogger<CommunicationAnalyticsService> _logger;
    private static readonly DistributedCacheEntryOptions CacheOptions = new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15) };

    public CommunicationAnalyticsService(
        ICommunicationAnalyticsRepository repository,
        IDistributedCache cache,
        ILogger<CommunicationAnalyticsService> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<AnalyticsOverviewDto> GetOverviewAsync(Guid organizationId)
    {
        var cacheKey = $"comm_analytics_overview_{organizationId}";
        try
        {
            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                var dto = JsonSerializer.Deserialize<AnalyticsOverviewDto>(cached);
                if (dto != null) return dto;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to read communication analytics from Redis cache.");
        }

        var overview = await _repository.GetOverviewAsync(organizationId);

        try
        {
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(overview), CacheOptions);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to write communication analytics to Redis cache.");
        }

        return overview;
    }

    public async Task<bool> SubmitCsatRatingAsync(Guid conversationId, Guid organizationId, SubmitCsatRequest request)
    {
        var result = await _repository.RecordCsatAsync(conversationId, organizationId, request.Rating, request.Feedback);
        if (result)
        {
            // Invalidate overview cache
            try
            {
                await _cache.RemoveAsync($"comm_analytics_overview_{organizationId}");
            }
            catch { /* Ignore redis errors */ }
        }
        return result;
    }

    public async Task RunDailyAggregationAsync()
    {
        await _repository.PerformDailyAggregationAsync();
    }
}
