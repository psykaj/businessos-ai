using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using backend.Common;
using backend.Modules.ProductAnalytics.DTOs;
using backend.Modules.ProductAnalytics.Entities;
using backend.Modules.ProductAnalytics.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace backend.Modules.ProductAnalytics.Services;

public class ProductAnalyticsService : IProductAnalyticsService
{
    private readonly IProductAnalyticsRepository _repository;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly ILogger<ProductAnalyticsService> _logger;

    public ProductAnalyticsService(
        IProductAnalyticsRepository repository,
        ApplicationDbContext context,
        IMapper mapper,
        IDistributedCache cache,
        ILogger<ProductAnalyticsService> logger)
    {
        _repository = repository;
        _context = context;
        _mapper = mapper;
        _cache = cache;
        _logger = logger;
    }

    public async Task<ProductPerformanceSummaryDto> GetSummaryAsync(Guid organizationId)
    {
        var cacheKey = $"ProductAnalytics_Summary_{organizationId}";
        try
        {
            var cachedJson = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedJson))
            {
                var cached = JsonSerializer.Deserialize<ProductPerformanceSummaryDto>(cachedJson);
                if (cached != null) return cached;
            }
        }
        catch { }

        var all = await _repository.GetAllActiveAsync(organizationId);
        if (all.Count == 0)
        {
            all = await SeedProductPerformancesAsync(organizationId);
        }

        var top = all.OrderByDescending(p => p.TotalProfit).Take(5).ToList();
        var least = all.OrderBy(p => p.TotalProfit).Take(5).ToList();

        var sumRev = all.Sum(p => p.RevenueGenerated);
        var avgMargin = all.Count > 0 ? Math.Round(all.Average(p => p.GrossMarginPercentage), 2) : 0;

        var summary = new ProductPerformanceSummaryDto
        {
            OrganizationId = organizationId,
            TotalProductsTracked = all.Count,
            TotalProductRevenue = sumRev,
            AverageProductMargin = avgMargin,
            TopPerformers = _mapper.Map<List<ProductPerformanceDto>>(top),
            LeastPerformers = _mapper.Map<List<ProductPerformanceDto>>(least)
        };

        try { await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(summary), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15) }); } catch { }

        return summary;
    }

    private async Task<List<ProductPerformance>> SeedProductPerformancesAsync(Guid organizationId)
    {
        var list = new List<ProductPerformance>();
        var seedNames = new[] { "Enterprise CRM License", "AI Automation Suite", "Advanced BI Addon", "Starter Plan", "Legacy Plugin" };
        var prices = new[] { 1200m, 850m, 500m, 49m, 15m };
        var costs = new[] { 200m, 150m, 80m, 25m, 12m };
        var units = new[] { 45, 30, 60, 12, 5 };

        for (int i = 0; i < seedNames.Length; i++)
        {
            var rev = prices[i] * units[i];
            var profit = (prices[i] - costs[i]) * units[i];
            var margin = prices[i] > 0 ? Math.Round(((prices[i] - costs[i]) / prices[i]) * 100, 2) : 0;

            var entity = new ProductPerformance
            {
                OrganizationId = organizationId,
                ProductId = Guid.NewGuid(),
                ProductSku = $"SKU-2026-00{i+1}",
                ProductName = seedNames[i],
                Category = i < 3 ? "Enterprise Software" : "Legacy Addons",
                Period = DateTime.UtcNow.ToString("yyyy-MM"),
                UnitsSold = units[i],
                UnitPrice = prices[i],
                UnitCost = costs[i],
                RevenueGenerated = rev,
                TotalProfit = profit,
                GrossMarginPercentage = margin,
                ReturnRatePercentage = i == 4 ? 14.5m : 1.2m,
                InventoryTurnoverRate = 4.2m,
                IsTopPerformer = i < 3,
                IsLeastPerformer = i >= 3,
                RecommendationNotes = i >= 3 ? "Consider deprecating or packaging with high-margin enterprise plans." : "High margin leader. Accelerate marketing spend."
            };
            await _repository.CreateAsync(entity);
            list.Add(entity);
        }
        return list;
    }

    public async Task<PagedResult<ProductPerformanceDto>> GetPerformancesAsync(Guid organizationId, string? period, string? category, bool? topPerformers, int pageNumber, int pageSize)
    {
        var res = await _repository.GetPerformancesAsync(organizationId, period, category, topPerformers, pageNumber, pageSize);
        var dtos = _mapper.Map<IEnumerable<ProductPerformanceDto>>(res.Items);
        return new PagedResult<ProductPerformanceDto>(dtos, res.TotalCount, pageNumber, pageSize);
    }

    public async Task<ProductPerformanceDto> RecordPerformanceAsync(Guid organizationId, CreateProductPerformanceRequest request, string? userId = null)
    {
        var rev = request.UnitPrice * request.UnitsSold;
        var profit = (request.UnitPrice - request.UnitCost) * request.UnitsSold;
        var margin = request.UnitPrice > 0 ? Math.Round(((request.UnitPrice - request.UnitCost) / request.UnitPrice) * 100, 2) : 0;

        var entity = new ProductPerformance
        {
            OrganizationId = organizationId,
            ProductId = request.ProductId == Guid.Empty ? Guid.NewGuid() : request.ProductId,
            ProductSku = request.ProductSku,
            ProductName = request.ProductName,
            Category = request.Category,
            Period = string.IsNullOrWhiteSpace(request.Period) ? DateTime.UtcNow.ToString("yyyy-MM") : request.Period,
            UnitsSold = request.UnitsSold,
            UnitPrice = request.UnitPrice,
            UnitCost = request.UnitCost,
            RevenueGenerated = rev,
            TotalProfit = profit,
            GrossMarginPercentage = margin,
            ReturnRatePercentage = request.ReturnRatePercentage,
            IsTopPerformer = margin > 60 && profit > 5000,
            IsLeastPerformer = margin < 25 || request.UnitsSold < 5,
            RecommendationNotes = request.RecommendationNotes,
            CreatedBy = userId
        };

        await _repository.CreateAsync(entity);
        try { await _cache.RemoveAsync($"ProductAnalytics_Summary_{organizationId}"); } catch { }
        return _mapper.Map<ProductPerformanceDto>(entity);
    }

    public async Task<bool> DeletePerformanceAsync(Guid id, Guid organizationId)
    {
        var res = await _repository.DeleteAsync(id, organizationId);
        if (res) { try { await _cache.RemoveAsync($"ProductAnalytics_Summary_{organizationId}"); } catch { } }
        return res;
    }

    public async Task<string> ExportProductReportAsync(Guid organizationId)
    {
        var all = await _repository.GetAllActiveAsync(organizationId);
        var sb = new StringBuilder();
        sb.AppendLine("ProductName,SKU,Category,UnitsSold,UnitPrice,UnitCost,RevenueGenerated,TotalProfit,Margin%,IsTopPerformer");
        foreach (var p in all)
        {
            sb.AppendLine($"{p.ProductName},{p.ProductSku},{p.Category},{p.UnitsSold},{p.UnitPrice},{p.UnitCost},{p.RevenueGenerated},{p.TotalProfit},{p.GrossMarginPercentage}%,{p.IsTopPerformer}");
        }
        return sb.ToString();
    }
}
