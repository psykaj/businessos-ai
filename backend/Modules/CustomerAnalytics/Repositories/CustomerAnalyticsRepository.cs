using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.CustomerAnalytics.Entities;
using backend.Modules.CustomerAnalytics.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerAnalytics.Repositories;

public class CustomerAnalyticsRepository : ICustomerAnalyticsRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerAnalyticsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerPerformance?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.CustomerPerformances
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && c.OrganizationId == organizationId && !c.IsDeleted);
    }

    public async Task<PagedResult<CustomerPerformance>> GetPerformancesAsync(Guid organizationId, string? segment, string? status, int pageNumber, int pageSize)
    {
        var query = _context.CustomerPerformances
            .AsNoTracking()
            .Where(c => c.OrganizationId == organizationId && !c.IsDeleted);

        if (!string.IsNullOrWhiteSpace(segment))
            query = query.Where(c => c.CustomerSegment == segment);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(c => c.Status == status);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(c => c.LifetimeValue)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<CustomerPerformance>(items, total, pageNumber, pageSize);
    }

    public async Task<List<CustomerPerformance>> GetAllActiveAsync(Guid organizationId)
    {
        return await _context.CustomerPerformances
            .AsNoTracking()
            .Where(c => c.OrganizationId == organizationId && !c.IsDeleted)
            .OrderByDescending(c => c.LifetimeValue)
            .ToListAsync();
    }

    public async Task<CustomerPerformance> CreateAsync(CustomerPerformance performance)
    {
        _context.CustomerPerformances.Add(performance);
        await _context.SaveChangesAsync();
        return performance;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid organizationId)
    {
        var item = await _context.CustomerPerformances
            .FirstOrDefaultAsync(c => c.Id == id && c.OrganizationId == organizationId && !c.IsDeleted);
        if (item == null) return false;

        item.IsDeleted = true;
        item.DeletedAt = DateTime.UtcNow;
        _context.CustomerPerformances.Update(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
