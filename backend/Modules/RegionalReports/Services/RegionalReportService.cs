using backend.Modules.RegionalReports.DTOs;
using backend.Modules.RegionalReports.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace backend.Modules.RegionalReports.Services;

public class RegionalReportService : IRegionalReportService
{
    private readonly IRegionalReportRepository _repository;
    private readonly IDistributedCache _cache;

    public RegionalReportService(IRegionalReportRepository repository, IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    private async Task<T?> GetCachedOrFetchAsync<T>(string key, Func<Task<T>> fetchFunc, CancellationToken cancellationToken) where T : class
    {
        var cached = await _cache.GetStringAsync(key, cancellationToken);
        if (!string.IsNullOrEmpty(cached))
        {
            try { return JsonSerializer.Deserialize<T>(cached); }
            catch { /* fallback */ }
        }

        var data = await fetchFunc();
        if (data != null)
        {
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(data), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            }, cancellationToken);
        }
        return data;
    }

    public async Task<IEnumerable<BranchRevenueSummaryDto>> GetRevenueByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"report_rev_{organizationId}_{year}_{month}_{region}";
        return (await GetCachedOrFetchAsync(cacheKey, () => _repository.GetRevenueByBranchAsync(organizationId, year, month, region, cancellationToken), cancellationToken)) ?? new List<BranchRevenueSummaryDto>();
    }

    public async Task<IEnumerable<BranchProfitSummaryDto>> GetProfitByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"report_profit_{organizationId}_{year}_{month}_{region}";
        return (await GetCachedOrFetchAsync(cacheKey, () => _repository.GetProfitByBranchAsync(organizationId, year, month, region, cancellationToken), cancellationToken)) ?? new List<BranchProfitSummaryDto>();
    }

    public async Task<IEnumerable<BranchInventorySummaryDto>> GetInventoryByBranchAsync(Guid organizationId, string? region = null, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"report_inv_{organizationId}_{region}";
        return (await GetCachedOrFetchAsync(cacheKey, () => _repository.GetInventoryByBranchAsync(organizationId, region, cancellationToken), cancellationToken)) ?? new List<BranchInventorySummaryDto>();
    }

    public async Task<IEnumerable<BranchCustomerSummaryDto>> GetCustomerCountByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"report_cust_{organizationId}_{year}_{month}_{region}";
        return (await GetCachedOrFetchAsync(cacheKey, () => _repository.GetCustomerCountByBranchAsync(organizationId, year, month, region, cancellationToken), cancellationToken)) ?? new List<BranchCustomerSummaryDto>();
    }

    public async Task<IEnumerable<BranchEmployeeSummaryDto>> GetEmployeeCountByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"report_emp_{organizationId}_{year}_{month}_{region}";
        return (await GetCachedOrFetchAsync(cacheKey, () => _repository.GetEmployeeCountByBranchAsync(organizationId, year, month, region, cancellationToken), cancellationToken)) ?? new List<BranchEmployeeSummaryDto>();
    }

    public async Task<IEnumerable<BranchRankingSummaryDto>> GetBranchRankingAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"report_rank_{organizationId}_{year}_{month}_{region}";
        return (await GetCachedOrFetchAsync(cacheKey, () => _repository.GetBranchRankingAsync(organizationId, year, month, region, cancellationToken), cancellationToken)) ?? new List<BranchRankingSummaryDto>();
    }
}
