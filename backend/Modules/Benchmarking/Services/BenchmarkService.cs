using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using backend.Common;
using backend.Modules.Benchmarking.DTOs;
using backend.Modules.Benchmarking.Entities;
using backend.Modules.Benchmarking.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace backend.Modules.Benchmarking.Services;

public class BenchmarkService : IBenchmarkService
{
    private readonly IBenchmarkRepository _repository;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly ILogger<BenchmarkService> _logger;

    public BenchmarkService(
        IBenchmarkRepository repository,
        ApplicationDbContext context,
        IMapper mapper,
        IDistributedCache cache,
        ILogger<BenchmarkService> logger)
    {
        _repository = repository;
        _context = context;
        _mapper = mapper;
        _cache = cache;
        _logger = logger;
    }

    public async Task<BenchmarkComparisonSummaryDto> GetComparisonSummaryAsync(Guid organizationId, string industry = "Enterprise SaaS & AI Software")
    {
        var cacheKey = $"Benchmarking_Summary_{organizationId}";
        try
        {
            var cachedJson = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedJson))
            {
                var cached = JsonSerializer.Deserialize<BenchmarkComparisonSummaryDto>(cachedJson);
                if (cached != null) return cached;
            }
        }
        catch { }

        var all = await _repository.GetAllActiveAsync(organizationId);
        if (all.Count == 0)
        {
            all = await SeedIndustryBenchmarksAsync(organizationId, industry);
        }

        var topCount = all.Count(b => b.OrganizationValue >= b.BenchmarkTopQuartile || b.PercentileRank >= 75);
        var abvMedian = all.Count(b => b.OrganizationValue >= b.BenchmarkMedian || b.PercentileRank >= 50);

        var summary = new BenchmarkComparisonSummaryDto
        {
            OrganizationId = organizationId,
            SelectedIndustry = industry,
            TotalMetricsCompared = all.Count,
            MetricsInTopQuartile = topCount,
            MetricsAboveMedian = abvMedian,
            Comparisons = _mapper.Map<List<BenchmarkMetricDto>>(all)
        };

        try { await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(summary), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15) }); } catch { }
        return summary;
    }

    private async Task<List<BenchmarkMetric>> SeedIndustryBenchmarksAsync(Guid organizationId, string industry)
    {
        var list = new List<BenchmarkMetric>();
        var names = new[] { "Annual Growth Rate (%)", "Gross Margin (%)", "LTV / CAC Ratio", "Net Revenue Retention (%)", "Customer Churn Rate (%)" };
        var orgVals = new[] { 45.5m, 81.2m, 6.4m, 112.4m, 3.2m };
        var medians = new[] { 32.0m, 72.0m, 3.5m, 104.0m, 6.5m };
        var tops = new[] { 65.0m, 80.0m, 5.0m, 120.0m, 2.5m };

        for (int i = 0; i < names.Length; i++)
        {
            var rank = 65m;
            if (i == 1 || i == 2) rank = 82m; // Top quartile in margin & ratio
            if (i == 4) rank = 78m; // lower churn is better

            var entity = new BenchmarkMetric
            {
                OrganizationId = organizationId,
                ComparisonType = industry,
                MetricName = names[i],
                OrganizationValue = orgVals[i],
                BenchmarkMedian = medians[i],
                BenchmarkTopQuartile = tops[i],
                PercentileRank = rank,
                Status = rank >= 75 ? "Top Quartile Leader" : (rank >= 50 ? "Above Industry Average" : "Needs Improvement")
            };
            await _repository.CreateAsync(entity);
            list.Add(entity);
        }
        return list;
    }

    public async Task<PagedResult<BenchmarkMetricDto>> GetMetricsAsync(Guid organizationId, string? comparisonType, int pageNumber, int pageSize)
    {
        var res = await _repository.GetMetricsAsync(organizationId, comparisonType, pageNumber, pageSize);
        var dtos = _mapper.Map<IEnumerable<BenchmarkMetricDto>>(res.Items);
        return new PagedResult<BenchmarkMetricDto>(dtos, res.TotalCount, pageNumber, pageSize);
    }

    public async Task<BenchmarkMetricDto> RecordMetricAsync(Guid organizationId, CreateBenchmarkMetricRequest request, string? userId = null)
    {
        var rank = request.OrganizationValue >= request.BenchmarkTopQuartile ? 85m : (request.OrganizationValue >= request.BenchmarkMedian ? 60m : 35m);
        var status = rank >= 75 ? "Top Quartile Leader" : (rank >= 50 ? "Above Industry Average" : "Below Industry Median");

        var entity = new BenchmarkMetric
        {
            OrganizationId = organizationId,
            ComparisonType = request.ComparisonType,
            MetricName = request.MetricName,
            OrganizationValue = request.OrganizationValue,
            BenchmarkMedian = request.BenchmarkMedian,
            BenchmarkTopQuartile = request.BenchmarkTopQuartile,
            PercentileRank = rank,
            Status = status,
            CreatedBy = userId
        };

        await _repository.CreateAsync(entity);
        try { await _cache.RemoveAsync($"Benchmarking_Summary_{organizationId}"); } catch { }
        return _mapper.Map<BenchmarkMetricDto>(entity);
    }

    public async Task<bool> DeleteMetricAsync(Guid id, Guid organizationId)
    {
        var ok = await _repository.DeleteAsync(id, organizationId);
        if (ok) { try { await _cache.RemoveAsync($"Benchmarking_Summary_{organizationId}"); } catch { } }
        return ok;
    }

    public async Task<string> ExportBenchmarkReportAsync(Guid organizationId)
    {
        var all = await _repository.GetAllActiveAsync(organizationId);
        var sb = new StringBuilder();
        sb.AppendLine("Industry,MetricName,OurValue,IndustryMedian,IndustryTopQuartile,PercentileRank,Status");
        foreach (var b in all)
        {
            sb.AppendLine($"{b.ComparisonType},{b.MetricName},{b.OrganizationValue},{b.BenchmarkMedian},{b.BenchmarkTopQuartile},{b.PercentileRank}%,{b.Status}");
        }
        return sb.ToString();
    }
}
