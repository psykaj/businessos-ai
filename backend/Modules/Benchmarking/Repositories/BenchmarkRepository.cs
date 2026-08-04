using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.Benchmarking.Entities;
using backend.Modules.Benchmarking.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Benchmarking.Repositories;

public class BenchmarkRepository : IBenchmarkRepository
{
    private readonly ApplicationDbContext _context;

    public BenchmarkRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BenchmarkMetric?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.BenchmarkMetrics
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id && b.OrganizationId == organizationId && !b.IsDeleted);
    }

    public async Task<PagedResult<BenchmarkMetric>> GetMetricsAsync(Guid organizationId, string? comparisonType, int pageNumber, int pageSize)
    {
        var query = _context.BenchmarkMetrics
            .AsNoTracking()
            .Where(b => b.OrganizationId == organizationId && !b.IsDeleted);

        if (!string.IsNullOrWhiteSpace(comparisonType))
            query = query.Where(b => b.ComparisonType == comparisonType);

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(b => b.MetricName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<BenchmarkMetric>(items, total, pageNumber, pageSize);
    }

    public async Task<List<BenchmarkMetric>> GetAllActiveAsync(Guid organizationId)
    {
        return await _context.BenchmarkMetrics
            .AsNoTracking()
            .Where(b => b.OrganizationId == organizationId && !b.IsDeleted)
            .OrderBy(b => b.MetricName)
            .ToListAsync();
    }

    public async Task<BenchmarkMetric> CreateAsync(BenchmarkMetric metric)
    {
        _context.BenchmarkMetrics.Add(metric);
        await _context.SaveChangesAsync();
        return metric;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid organizationId)
    {
        var item = await _context.BenchmarkMetrics
            .FirstOrDefaultAsync(b => b.Id == id && b.OrganizationId == organizationId && !b.IsDeleted);
        if (item == null) return false;

        item.IsDeleted = true;
        item.DeletedAt = DateTime.UtcNow;
        _context.BenchmarkMetrics.Update(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
