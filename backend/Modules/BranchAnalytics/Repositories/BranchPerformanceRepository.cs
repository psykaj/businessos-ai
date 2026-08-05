using Microsoft.EntityFrameworkCore;
using backend.Modules.BranchAnalytics.Entities;
using backend.Modules.BranchAnalytics.Interfaces;
using backend.Persistence;

namespace backend.Modules.BranchAnalytics.Repositories;

public class BranchPerformanceRepository : IBranchPerformanceRepository
{
    private readonly ApplicationDbContext _context;

    public BranchPerformanceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BranchPerformance>> GetByOrgAndPeriodAsync(Guid organizationId, int year, int month, CancellationToken cancellationToken = default)
    {
        return await _context.BranchPerformances
            .Where(x => x.OrganizationId == organizationId && x.Year == year && x.Month == month && !x.IsDeleted)
            .OrderBy(x => x.Rank)
            .ToListAsync(cancellationToken);
    }

    public async Task<BranchPerformance?> GetByBranchAndPeriodAsync(Guid branchId, int year, int month, CancellationToken cancellationToken = default)
    {
        return await _context.BranchPerformances
            .Where(x => x.BranchId == branchId && x.Year == year && x.Month == month && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<BranchPerformance> AddOrUpdateAsync(BranchPerformance performance, CancellationToken cancellationToken = default)
    {
        var existing = await _context.BranchPerformances
            .Where(x => x.OrganizationId == performance.OrganizationId && x.BranchId == performance.BranchId && x.Year == performance.Year && x.Month == performance.Month && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (existing == null)
        {
            await _context.BranchPerformances.AddAsync(performance, cancellationToken);
        }
        else
        {
            existing.BranchName = performance.BranchName;
            existing.TotalRevenue = performance.TotalRevenue;
            existing.TotalExpenses = performance.TotalExpenses;
            existing.TotalProfit = performance.TotalProfit;
            existing.ProfitMarginPercentage = performance.ProfitMarginPercentage;
            existing.CustomerCount = performance.CustomerCount;
            existing.EmployeeCount = performance.EmployeeCount;
            existing.InventoryUtilizationPercentage = performance.InventoryUtilizationPercentage;
            existing.PerformanceScore = performance.PerformanceScore;
            existing.Rank = performance.Rank;
            existing.IsBestPerformer = performance.IsBestPerformer;
            existing.IsLowestPerformer = performance.IsLowestPerformer;
            existing.MoMRevenueGrowthPercentage = performance.MoMRevenueGrowthPercentage;
            existing.MoMProfitGrowthPercentage = performance.MoMProfitGrowthPercentage;
            _context.BranchPerformances.Update(existing);
            performance = existing;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return performance;
    }
}
