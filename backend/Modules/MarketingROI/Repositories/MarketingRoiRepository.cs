using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.MarketingROI.Entities;
using backend.Modules.MarketingROI.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.MarketingROI.Repositories;

public class MarketingRoiRepository : IMarketingRoiRepository
{
    private readonly ApplicationDbContext _context;

    public MarketingRoiRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MarketingPerformance?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.MarketingPerformances
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id && m.OrganizationId == organizationId && !m.IsDeleted);
    }

    public async Task<PagedResult<MarketingPerformance>> GetPerformancesAsync(Guid organizationId, string? channel, string? period, int pageNumber, int pageSize)
    {
        var query = _context.MarketingPerformances
            .AsNoTracking()
            .Where(m => m.OrganizationId == organizationId && !m.IsDeleted);

        if (!string.IsNullOrWhiteSpace(channel))
            query = query.Where(m => m.Channel == channel);

        if (!string.IsNullOrWhiteSpace(period))
            query = query.Where(m => m.Period == period);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(m => m.ReturnOnAdSpend)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<MarketingPerformance>(items, total, pageNumber, pageSize);
    }

    public async Task<List<MarketingPerformance>> GetAllActiveAsync(Guid organizationId)
    {
        return await _context.MarketingPerformances
            .AsNoTracking()
            .Where(m => m.OrganizationId == organizationId && !m.IsDeleted)
            .OrderByDescending(m => m.ReturnOnAdSpend)
            .ToListAsync();
    }

    public async Task<MarketingPerformance> CreateAsync(MarketingPerformance performance)
    {
        _context.MarketingPerformances.Add(performance);
        await _context.SaveChangesAsync();
        return performance;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid organizationId)
    {
        var item = await _context.MarketingPerformances
            .FirstOrDefaultAsync(m => m.Id == id && m.OrganizationId == organizationId && !m.IsDeleted);
        if (item == null) return false;

        item.IsDeleted = true;
        item.DeletedAt = DateTime.UtcNow;
        _context.MarketingPerformances.Update(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
