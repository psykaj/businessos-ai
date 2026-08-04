using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.BusinessPerformance.Entities;
using backend.Modules.BusinessPerformance.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BusinessPerformance.Repositories;

public class BusinessPerformanceRepository : IBusinessPerformanceRepository
{
    private readonly ApplicationDbContext _context;

    public BusinessPerformanceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BusinessMetric?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.BusinessMetrics
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id && m.OrganizationId == organizationId && !m.IsDeleted);
    }

    public async Task<PagedResult<BusinessMetric>> GetMetricsAsync(Guid organizationId, string? metricType, string? category, int pageNumber, int pageSize)
    {
        var query = _context.BusinessMetrics
            .AsNoTracking()
            .Where(m => m.OrganizationId == organizationId && !m.IsDeleted);

        if (!string.IsNullOrWhiteSpace(metricType))
            query = query.Where(m => m.MetricType == metricType);

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(m => m.Category == category);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(m => m.CalculatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<BusinessMetric>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<List<BusinessMetric>> GetLatestMetricsByPeriodAsync(Guid organizationId, string period)
    {
        return await _context.BusinessMetrics
            .AsNoTracking()
            .Where(m => m.OrganizationId == organizationId && m.Period == period && !m.IsDeleted)
            .OrderByDescending(m => m.CalculatedAt)
            .ToListAsync();
    }

    public async Task<BusinessMetric> CreateAsync(BusinessMetric metric)
    {
        _context.BusinessMetrics.Add(metric);
        await _context.SaveChangesAsync();
        return metric;
    }

    public async Task<BusinessMetric> UpdateAsync(BusinessMetric metric)
    {
        _context.BusinessMetrics.Update(metric);
        await _context.SaveChangesAsync();
        return metric;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid organizationId)
    {
        var metric = await _context.BusinessMetrics
            .FirstOrDefaultAsync(m => m.Id == id && m.OrganizationId == organizationId && !m.IsDeleted);

        if (metric == null) return false;

        metric.IsDeleted = true;
        metric.DeletedAt = DateTime.UtcNow;
        _context.BusinessMetrics.Update(metric);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<BusinessMetric>> GetAllActiveByOrgAsync(Guid organizationId)
    {
        return await _context.BusinessMetrics
            .AsNoTracking()
            .Where(m => m.OrganizationId == organizationId && !m.IsDeleted)
            .OrderByDescending(m => m.CalculatedAt)
            .ToListAsync();
    }
}
