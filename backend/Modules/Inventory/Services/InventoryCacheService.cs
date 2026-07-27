using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace backend.Modules.Inventory.Services;

public class InventoryCacheService
{
    private readonly IDistributedCache? _cache;
    private readonly ILogger<InventoryCacheService> _logger;

    public InventoryCacheService(ILogger<InventoryCacheService> logger, IDistributedCache? cache = null)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (_cache == null) return default;
        try
        {
            var value = await _cache.GetStringAsync(key, cancellationToken);
            if (string.IsNullOrEmpty(value)) return default;
            return JsonSerializer.Deserialize<T>(value);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to read key {Key} from distributed cache", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan absoluteExpiration, CancellationToken cancellationToken = default)
    {
        if (_cache == null) return;
        try
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration
            };
            var json = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, json, options, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to write key {Key} to distributed cache", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        if (_cache == null) return;
        try
        {
            await _cache.RemoveAsync(key, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to remove key {Key} from distributed cache", key);
        }
    }
}
