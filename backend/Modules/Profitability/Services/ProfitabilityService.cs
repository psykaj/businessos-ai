using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using backend.Common;
using backend.Modules.Profitability.DTOs;
using backend.Modules.Profitability.Entities;
using backend.Modules.Profitability.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace backend.Modules.Profitability.Services;

public class ProfitabilityService : IProfitabilityService
{
    private readonly IProfitabilityRepository _repository;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly ILogger<ProfitabilityService> _logger;

    public ProfitabilityService(
        IProfitabilityRepository repository,
        ApplicationDbContext context,
        IMapper mapper,
        IDistributedCache cache,
        ILogger<ProfitabilityService> logger)
    {
        _repository = repository;
        _context = context;
        _mapper = mapper;
        _cache = cache;
        _logger = logger;
    }

    public async Task<ProfitabilityAnalysisDto> GetAnalysisAsync(Guid organizationId)
    {
        var cacheKey = $"Profitability_Analysis_{organizationId}";
        try
        {
            var cachedJson = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedJson))
            {
                var cached = JsonSerializer.Deserialize<ProfitabilityAnalysisDto>(cachedJson);
                if (cached != null) return cached;
            }
        }
        catch { }

        var snapshots = await _repository.GetRecentSnapshotsAsync(organizationId, 12);
        if (snapshots.Count == 0)
        {
            snapshots = await GenerateSeedSnapshotsAsync(organizationId);
        }

        var latest = snapshots.OrderByDescending(s => s.SnapshotDate).First();
        var dtos = _mapper.Map<List<ProfitSnapshotDto>>(snapshots);

        // Analyze products to distinguish profit centers vs loss centers
        var products = await _context.Products.AsNoTracking().Where(p => p.OrganizationId == organizationId && !p.IsDeleted).ToListAsync();
        var profitCenters = new List<ProfitCenterItemDto>();
        var lossCenters = new List<ProfitCenterItemDto>();

        if (products.Count > 0)
        {
            foreach (var p in products)
            {
                var estCost = p.CostPrice > 0 ? p.CostPrice : p.SellingPrice * 0.45m;
                var margin = p.SellingPrice > 0 ? Math.Round(((p.SellingPrice - estCost) / p.SellingPrice) * 100, 2) : 0;
                var item = new ProfitCenterItemDto
                {
                    Name = p.Name,
                    Category = "Product",
                    Revenue = p.SellingPrice * 25,
                    Cost = estCost * 25,
                    MarginPercentage = margin,
                    IsLossCenter = margin < 25m
                };
                if (item.IsLossCenter) lossCenters.Add(item); else profitCenters.Add(item);
            }
        }
        else
        {
            profitCenters.Add(new ProfitCenterItemDto { Name = "Enterprise SaaS Subscription", Revenue = 85000m, Cost = 15000m, MarginPercentage = 82.35m, IsLossCenter = false });
            profitCenters.Add(new ProfitCenterItemDto { Name = "AI Workflow Copilot Add-On", Revenue = 32000m, Cost = 6000m, MarginPercentage = 81.25m, IsLossCenter = false });
            lossCenters.Add(new ProfitCenterItemDto { Name = "Legacy Custom Integration Support", Revenue = 8000m, Cost = 11500m, MarginPercentage = -43.75m, IsLossCenter = true });
        }

        var analysis = new ProfitabilityAnalysisDto
        {
            OrganizationId = organizationId,
            TotalGrossRevenue = latest.GrossRevenue,
            TotalCostOfGoodsSold = latest.CostOfGoodsSold,
            OverallGrossMargin = latest.GrossMarginPercentage,
            TotalOperatingExpenses = latest.OperatingExpenses,
            OverallNetMargin = latest.NetMarginPercentage,
            ProfitCenters = profitCenters.OrderByDescending(x => x.MarginPercentage).ToList(),
            LossCenters = lossCenters.OrderBy(x => x.MarginPercentage).ToList(),
            HistoricalSnapshots = dtos
        };

        try
        {
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(analysis), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15) });
        }
        catch { }

        return analysis;
    }

    private async Task<List<ProfitSnapshot>> GenerateSeedSnapshotsAsync(Guid organizationId)
    {
        var list = new List<ProfitSnapshot>();
        for (int i = 5; i >= 0; i--)
        {
            var date = DateTime.UtcNow.AddMonths(-i);
            var rev = 45000m + (5 - i) * 3500m;
            var cogs = rev * 0.28m;
            var opex = rev * 0.42m;
            var grossProfit = rev - cogs;
            var netProfit = grossProfit - opex;

            var snap = new ProfitSnapshot
            {
                OrganizationId = organizationId,
                SnapshotDate = new DateTime(date.Year, date.Month, 1),
                Period = date.ToString("yyyy-MM"),
                GrossRevenue = rev,
                CostOfGoodsSold = cogs,
                GrossProfit = grossProfit,
                GrossMarginPercentage = Math.Round((grossProfit / rev) * 100, 2),
                OperatingExpenses = opex,
                NetProfit = netProfit,
                NetMarginPercentage = Math.Round((netProfit / rev) * 100, 2),
                Ebitda = netProfit * 1.12m
            };
            await _repository.CreateAsync(snap);
            list.Add(snap);
        }
        return list;
    }

    public async Task<PagedResult<ProfitSnapshotDto>> GetSnapshotsAsync(Guid organizationId, string? period, int pageNumber, int pageSize)
    {
        var paged = await _repository.GetSnapshotsAsync(organizationId, period, pageNumber, pageSize);
        var dtos = _mapper.Map<IEnumerable<ProfitSnapshotDto>>(paged.Items);
        return new PagedResult<ProfitSnapshotDto>(dtos, paged.TotalCount, pageNumber, pageSize);
    }

    public async Task<ProfitSnapshotDto> RecordSnapshotAsync(Guid organizationId, CreateProfitSnapshotRequest request, string? userId = null)
    {
        var grossProfit = request.GrossRevenue - request.CostOfGoodsSold;
        var netProfit = grossProfit - request.OperatingExpenses;
        var grossMargin = request.GrossRevenue > 0 ? Math.Round((grossProfit / request.GrossRevenue) * 100, 2) : 0;
        var netMargin = request.GrossRevenue > 0 ? Math.Round((netProfit / request.GrossRevenue) * 100, 2) : 0;

        var snap = new ProfitSnapshot
        {
            OrganizationId = organizationId,
            SnapshotDate = request.SnapshotDate,
            Period = string.IsNullOrWhiteSpace(request.Period) ? request.SnapshotDate.ToString("yyyy-MM") : request.Period,
            GrossRevenue = request.GrossRevenue,
            CostOfGoodsSold = request.CostOfGoodsSold,
            GrossProfit = grossProfit,
            GrossMarginPercentage = grossMargin,
            OperatingExpenses = request.OperatingExpenses,
            NetProfit = netProfit,
            NetMarginPercentage = netMargin,
            Ebitda = request.Ebitda > 0 ? request.Ebitda : netProfit * 1.1m,
            ProfitCenterBreakdownJson = request.ProfitCenterBreakdownJson,
            CreatedBy = userId
        };

        await _repository.CreateAsync(snap);
        try { await _cache.RemoveAsync($"Profitability_Analysis_{organizationId}"); } catch { }
        return _mapper.Map<ProfitSnapshotDto>(snap);
    }

    public async Task<bool> DeleteSnapshotAsync(Guid id, Guid organizationId)
    {
        var success = await _repository.DeleteAsync(id, organizationId);
        if (success) { try { await _cache.RemoveAsync($"Profitability_Analysis_{organizationId}"); } catch { } }
        return success;
    }

    public async Task<string> ExportProfitReportAsync(Guid organizationId)
    {
        var snapshots = await _repository.GetRecentSnapshotsAsync(organizationId, 24);
        var sb = new StringBuilder();
        sb.AppendLine("SnapshotDate,Period,GrossRevenue,COGS,GrossProfit,GrossMargin%,OperatingExpenses,NetProfit,NetMargin%,EBITDA");
        foreach (var s in snapshots)
        {
            sb.AppendLine($"{s.SnapshotDate:yyyy-MM-dd},{s.Period},{s.GrossRevenue},{s.CostOfGoodsSold},{s.GrossProfit},{s.GrossMarginPercentage}%,{s.OperatingExpenses},{s.NetProfit},{s.NetMarginPercentage}%,{s.Ebitda}");
        }
        return sb.ToString();
    }
}
