using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.Profitability.Entities;
using backend.Modules.Profitability.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Profitability.Repositories;

public class ProfitabilityRepository : IProfitabilityRepository
{
    private readonly ApplicationDbContext _context;

    public ProfitabilityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProfitSnapshot?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.ProfitSnapshots
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.OrganizationId == organizationId && !s.IsDeleted);
    }

    public async Task<PagedResult<ProfitSnapshot>> GetSnapshotsAsync(Guid organizationId, string? period, int pageNumber, int pageSize)
    {
        var query = _context.ProfitSnapshots
            .AsNoTracking()
            .Where(s => s.OrganizationId == organizationId && !s.IsDeleted);

        if (!string.IsNullOrWhiteSpace(period))
            query = query.Where(s => s.Period == period);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(s => s.SnapshotDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<ProfitSnapshot>(items, total, pageNumber, pageSize);
    }

    public async Task<List<ProfitSnapshot>> GetRecentSnapshotsAsync(Guid organizationId, int limit = 12)
    {
        return await _context.ProfitSnapshots
            .AsNoTracking()
            .Where(s => s.OrganizationId == organizationId && !s.IsDeleted)
            .OrderByDescending(s => s.SnapshotDate)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<ProfitSnapshot> CreateAsync(ProfitSnapshot snapshot)
    {
        _context.ProfitSnapshots.Add(snapshot);
        await _context.SaveChangesAsync();
        return snapshot;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid organizationId)
    {
        var item = await _context.ProfitSnapshots
            .FirstOrDefaultAsync(s => s.Id == id && s.OrganizationId == organizationId && !s.IsDeleted);
        if (item == null) return false;

        item.IsDeleted = true;
        item.DeletedAt = DateTime.UtcNow;
        _context.ProfitSnapshots.Update(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
