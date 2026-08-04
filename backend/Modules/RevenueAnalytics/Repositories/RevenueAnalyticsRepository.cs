using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.RevenueAnalytics.Entities;
using backend.Modules.RevenueAnalytics.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.RevenueAnalytics.Repositories;

public class RevenueAnalyticsRepository : IRevenueAnalyticsRepository
{
    private readonly ApplicationDbContext _context;

    public RevenueAnalyticsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RevenueSnapshot?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.RevenueSnapshots
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.OrganizationId == organizationId && !s.IsDeleted);
    }

    public async Task<PagedResult<RevenueSnapshot>> GetSnapshotsAsync(Guid organizationId, string? periodType, int pageNumber, int pageSize)
    {
        var query = _context.RevenueSnapshots
            .AsNoTracking()
            .Where(s => s.OrganizationId == organizationId && !s.IsDeleted);

        if (!string.IsNullOrWhiteSpace(periodType))
            query = query.Where(s => s.PeriodType == periodType);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(s => s.SnapshotDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<RevenueSnapshot>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<List<RevenueSnapshot>> GetRecentSnapshotsAsync(Guid organizationId, int limit = 12)
    {
        return await _context.RevenueSnapshots
            .AsNoTracking()
            .Where(s => s.OrganizationId == organizationId && !s.IsDeleted)
            .OrderByDescending(s => s.SnapshotDate)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<RevenueSnapshot> CreateAsync(RevenueSnapshot snapshot)
    {
        _context.RevenueSnapshots.Add(snapshot);
        await _context.SaveChangesAsync();
        return snapshot;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid organizationId)
    {
        var snapshot = await _context.RevenueSnapshots
            .FirstOrDefaultAsync(s => s.Id == id && s.OrganizationId == organizationId && !s.IsDeleted);

        if (snapshot == null) return false;

        snapshot.IsDeleted = true;
        snapshot.DeletedAt = DateTime.UtcNow;
        _context.RevenueSnapshots.Update(snapshot);
        await _context.SaveChangesAsync();
        return true;
    }
}
