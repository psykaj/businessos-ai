using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.ServiceQuality.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerFeedback.ServiceQuality.Repositories;

public class ServiceQualityRepository : IServiceQualityRepository
{
    private readonly ApplicationDbContext _context;

    public ServiceQualityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceMetric?> GetByDateAsync(Guid organizationId, DateTime date)
    {
        var target = date.Date;
        return await _context.ServiceMetrics
            .FirstOrDefaultAsync(m => m.OrganizationId == organizationId && m.MetricDate == target && !m.IsDeleted);
    }

    public async Task<IEnumerable<ServiceMetric>> GetHistoryAsync(Guid organizationId, int days)
    {
        var cutoff = DateTime.UtcNow.Date.AddDays(-days);
        return await _context.ServiceMetrics
            .AsNoTracking()
            .Where(m => m.OrganizationId == organizationId && m.MetricDate >= cutoff && !m.IsDeleted)
            .OrderByDescending(m => m.MetricDate)
            .ToListAsync();
    }

    public async Task<ServiceMetric> AddOrUpdateAsync(ServiceMetric metric)
    {
        var existing = await GetByDateAsync(metric.OrganizationId, metric.MetricDate);
        if (existing == null)
        {
            await _context.ServiceMetrics.AddAsync(metric);
        }
        else
        {
            existing.AverageFirstResponseTimeMinutes = metric.AverageFirstResponseTimeMinutes;
            existing.AverageResolutionTimeMinutes = metric.AverageResolutionTimeMinutes;
            existing.FirstContactResolutionRate = metric.FirstContactResolutionRate;
            existing.TotalResolvedTickets = metric.TotalResolvedTickets;
            _context.ServiceMetrics.Update(existing);
            metric = existing;
        }
        await _context.SaveChangesAsync();
        return metric;
    }
}
