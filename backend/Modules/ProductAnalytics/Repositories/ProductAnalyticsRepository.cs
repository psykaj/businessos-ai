using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.ProductAnalytics.Entities;
using backend.Modules.ProductAnalytics.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.ProductAnalytics.Repositories;

public class ProductAnalyticsRepository : IProductAnalyticsRepository
{
    private readonly ApplicationDbContext _context;

    public ProductAnalyticsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductPerformance?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.ProductPerformances
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id && p.OrganizationId == organizationId && !p.IsDeleted);
    }

    public async Task<PagedResult<ProductPerformance>> GetPerformancesAsync(Guid organizationId, string? period, string? category, bool? topPerformers, int pageNumber, int pageSize)
    {
        var query = _context.ProductPerformances
            .AsNoTracking()
            .Where(p => p.OrganizationId == organizationId && !p.IsDeleted);

        if (!string.IsNullOrWhiteSpace(period))
            query = query.Where(p => p.Period == period);

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(p => p.Category == category);

        if (topPerformers.HasValue)
        {
            if (topPerformers.Value) query = query.Where(p => p.IsTopPerformer);
            else query = query.Where(p => p.IsLeastPerformer);
        }

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(p => p.TotalProfit)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<ProductPerformance>(items, total, pageNumber, pageSize);
    }

    public async Task<List<ProductPerformance>> GetTopPerformersAsync(Guid organizationId, int limit = 5)
    {
        return await _context.ProductPerformances
            .AsNoTracking()
            .Where(p => p.OrganizationId == organizationId && !p.IsDeleted)
            .OrderByDescending(p => p.TotalProfit)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<ProductPerformance>> GetLeastPerformersAsync(Guid organizationId, int limit = 5)
    {
        return await _context.ProductPerformances
            .AsNoTracking()
            .Where(p => p.OrganizationId == organizationId && !p.IsDeleted)
            .OrderBy(p => p.TotalProfit)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<ProductPerformance> CreateAsync(ProductPerformance performance)
    {
        _context.ProductPerformances.Add(performance);
        await _context.SaveChangesAsync();
        return performance;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid organizationId)
    {
        var item = await _context.ProductPerformances
            .FirstOrDefaultAsync(p => p.Id == id && p.OrganizationId == organizationId && !p.IsDeleted);
        if (item == null) return false;

        item.IsDeleted = true;
        item.DeletedAt = DateTime.UtcNow;
        _context.ProductPerformances.Update(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ProductPerformance>> GetAllActiveAsync(Guid organizationId)
    {
        return await _context.ProductPerformances
            .AsNoTracking()
            .Where(p => p.OrganizationId == organizationId && !p.IsDeleted)
            .OrderByDescending(p => p.TotalProfit)
            .ToListAsync();
    }
}
