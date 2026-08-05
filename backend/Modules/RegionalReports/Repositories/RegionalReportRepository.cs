using Microsoft.EntityFrameworkCore;
using backend.Modules.RegionalReports.DTOs;
using backend.Modules.RegionalReports.Interfaces;
using backend.Persistence;

namespace backend.Modules.RegionalReports.Repositories;

public class RegionalReportRepository : IRegionalReportRepository
{
    private readonly ApplicationDbContext _context;

    public RegionalReportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BranchRevenueSummaryDto>> GetRevenueByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default)
    {
        var query = from p in _context.BranchPerformances
                    join b in _context.Branches on p.BranchId equals b.Id
                    join l in _context.Locations on b.LocationId equals l.Id into locs
                    from loc in locs.DefaultIfEmpty()
                    where p.OrganizationId == organizationId && p.Year == year && p.Month == month && !p.IsDeleted && !b.IsDeleted
                    select new { p, b, loc };

        if (!string.IsNullOrEmpty(region))
        {
            query = query.Where(x => x.loc != null && x.loc.Region.ToLower() == region.ToLower());
        }

        var results = await query.ToListAsync(cancellationToken);
        return results.Select(x => new BranchRevenueSummaryDto(
            x.b.Id,
            x.b.Name,
            x.loc?.Region ?? "Unassigned",
            x.p.TotalRevenue,
            x.p.MoMRevenueGrowthPercentage
        )).OrderByDescending(r => r.TotalRevenue);
    }

    public async Task<IEnumerable<BranchProfitSummaryDto>> GetProfitByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default)
    {
        var query = from p in _context.BranchPerformances
                    join b in _context.Branches on p.BranchId equals b.Id
                    join l in _context.Locations on b.LocationId equals l.Id into locs
                    from loc in locs.DefaultIfEmpty()
                    where p.OrganizationId == organizationId && p.Year == year && p.Month == month && !p.IsDeleted && !b.IsDeleted
                    select new { p, b, loc };

        if (!string.IsNullOrEmpty(region))
        {
            query = query.Where(x => x.loc != null && x.loc.Region.ToLower() == region.ToLower());
        }

        var results = await query.ToListAsync(cancellationToken);
        return results.Select(x => new BranchProfitSummaryDto(
            x.b.Id,
            x.b.Name,
            x.loc?.Region ?? "Unassigned",
            x.p.TotalRevenue,
            x.p.TotalExpenses,
            x.p.TotalProfit,
            x.p.ProfitMarginPercentage
        )).OrderByDescending(p => p.TotalProfit);
    }

    public async Task<IEnumerable<BranchInventorySummaryDto>> GetInventoryByBranchAsync(Guid organizationId, string? region = null, CancellationToken cancellationToken = default)
    {
        var branches = await (from b in _context.Branches
                              join l in _context.Locations on b.LocationId equals l.Id into locs
                              from loc in locs.DefaultIfEmpty()
                              where b.OrganizationId == organizationId && !b.IsDeleted
                              select new { b, loc }).ToListAsync(cancellationToken);

        if (!string.IsNullOrEmpty(region))
        {
            branches = branches.Where(x => x.loc != null && x.loc.Region.Equals(region, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var warehouses = await _context.BranchWarehouses
            .Where(w => w.OrganizationId == organizationId && !w.IsDeleted)
            .ToListAsync(cancellationToken);

        var summaries = new List<BranchInventorySummaryDto>();
        foreach (var item in branches)
        {
            var branchWhs = warehouses.Where(w => w.BranchId == item.b.Id).ToList();
            int whCount = branchWhs.Count;
            int totalItems = branchWhs.Sum(w => w.TotalStockItemsCount);
            decimal totalVal = branchWhs.Sum(w => w.EstimatedStockValue);
            decimal avgUtil = whCount > 0 ? Math.Round(branchWhs.Average(w => w.CurrentUtilizationPercentage), 2) : 0m;

            summaries.Add(new BranchInventorySummaryDto(item.b.Id, item.b.Name, whCount, totalItems, totalVal, avgUtil));
        }

        return summaries.OrderByDescending(s => s.TotalStockValue);
    }

    public async Task<IEnumerable<BranchCustomerSummaryDto>> GetCustomerCountByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default)
    {
        var query = from p in _context.BranchPerformances
                    join b in _context.Branches on p.BranchId equals b.Id
                    where p.OrganizationId == organizationId && p.Year == year && p.Month == month && !p.IsDeleted
                    select new { p, b };

        var results = await query.ToListAsync(cancellationToken);
        return results.Select(x => new BranchCustomerSummaryDto(
            x.b.Id,
            x.b.Name,
            x.p.CustomerCount,
            x.p.CustomerCount > 0 ? Math.Round(x.p.TotalRevenue / x.p.CustomerCount, 2) : 0m
        )).OrderByDescending(c => c.TotalCustomers);
    }

    public async Task<IEnumerable<BranchEmployeeSummaryDto>> GetEmployeeCountByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default)
    {
        var query = from p in _context.BranchPerformances
                    join b in _context.Branches on p.BranchId equals b.Id
                    where p.OrganizationId == organizationId && p.Year == year && p.Month == month && !p.IsDeleted
                    select new { p, b };

        var results = await query.ToListAsync(cancellationToken);
        return results.Select(x => new BranchEmployeeSummaryDto(
            x.b.Id,
            x.b.Name,
            x.p.EmployeeCount,
            x.p.EmployeeCount > 0 ? Math.Round(x.p.TotalRevenue / x.p.EmployeeCount, 2) : 0m
        )).OrderByDescending(e => e.TotalEmployees);
    }

    public async Task<IEnumerable<BranchRankingSummaryDto>> GetBranchRankingAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default)
    {
        var query = from p in _context.BranchPerformances
                    join b in _context.Branches on p.BranchId equals b.Id
                    join l in _context.Locations on b.LocationId equals l.Id into locs
                    from loc in locs.DefaultIfEmpty()
                    where p.OrganizationId == organizationId && p.Year == year && p.Month == month && !p.IsDeleted
                    select new { p, b, loc };

        if (!string.IsNullOrEmpty(region))
        {
            query = query.Where(x => x.loc != null && x.loc.Region.ToLower() == region.ToLower());
        }

        var results = await query.OrderByDescending(x => x.p.PerformanceScore).ToListAsync(cancellationToken);
        var summaries = new List<BranchRankingSummaryDto>();
        int rank = 1;
        foreach (var r in results)
        {
            summaries.Add(new BranchRankingSummaryDto(
                rank++,
                r.b.Id,
                r.b.Name,
                r.loc?.Region ?? "Unassigned",
                r.p.PerformanceScore,
                r.p.TotalRevenue,
                r.p.TotalProfit,
                r.p.IsBestPerformer,
                r.p.IsLowestPerformer
            ));
        }
        return summaries;
    }
}
