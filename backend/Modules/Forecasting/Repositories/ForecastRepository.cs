using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.Forecasting.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Forecasting.Repositories;

public class ForecastRepository : IForecastRepository
{
    private readonly ApplicationDbContext _context;

    public ForecastRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Forecast>> GetAllAsync(Guid organizationId)
    {
        return await _context.Forecasts
            .Where(f => f.OrganizationId == organizationId && !f.IsDeleted)
            .OrderByDescending(f => f.ForecastDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Forecast>> GetByMetricAsync(string metricName, Guid organizationId)
    {
        return await _context.Forecasts
            .Where(f => f.MetricName == metricName && f.OrganizationId == organizationId && !f.IsDeleted)
            .OrderBy(f => f.ForecastDate)
            .ToListAsync();
    }

    public async Task<Forecast> AddAsync(Forecast forecast)
    {
        _context.Forecasts.Add(forecast);
        await _context.SaveChangesAsync();
        return forecast;
    }

    public async Task AddRangeAsync(IEnumerable<Forecast> forecasts)
    {
        _context.Forecasts.AddRange(forecasts);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOldForecastsAsync(string metricName, Guid organizationId)
    {
        var oldForecasts = await _context.Forecasts
            .Where(f => f.MetricName == metricName && f.OrganizationId == organizationId && !f.IsDeleted)
            .ToListAsync();

        foreach (var f in oldForecasts)
        {
            f.IsDeleted = true;
        }
        
        _context.Forecasts.UpdateRange(oldForecasts);
        await _context.SaveChangesAsync();
    }
}
