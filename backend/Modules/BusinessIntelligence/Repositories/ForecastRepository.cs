using backend.Modules.BusinessIntelligence.Entities;
using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BusinessIntelligence.Repositories;

public class ForecastRepository : IForecastRepository
{
    private readonly ApplicationDbContext _context;

    public ForecastRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Forecast?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Forecasts
            .FirstOrDefaultAsync(f => f.Id == id && f.OrganizationId == organizationId, cancellationToken);
    }

    public async Task<List<Forecast>> GetByForecastTypeAsync(Guid organizationId, string forecastType, CancellationToken cancellationToken = default)
    {
        return await _context.Forecasts
            .AsNoTracking()
            .Where(f => f.OrganizationId == organizationId && f.ForecastType.ToLower() == forecastType.ToLower())
            .OrderBy(f => f.ForecastDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<(List<Forecast> Items, int TotalCount)> GetPagedAsync(
        Guid organizationId, 
        int page, 
        int pageSize, 
        string? forecastType = null, 
        string? sortBy = null, 
        bool sortDescending = false, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.Forecasts.AsNoTracking().Where(f => f.OrganizationId == organizationId);

        if (!string.IsNullOrWhiteSpace(forecastType))
        {
            query = query.Where(f => f.ForecastType.ToLower() == forecastType.ToLower());
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = sortBy?.ToLower() switch
        {
            "predictedvalue" => sortDescending ? query.OrderByDescending(f => f.PredictedValue) : query.OrderBy(f => f.PredictedValue),
            "forecastdate" => sortDescending ? query.OrderByDescending(f => f.ForecastDate) : query.OrderBy(f => f.ForecastDate),
            _ => sortDescending ? query.OrderByDescending(f => f.CreatedAt) : query.OrderBy(f => f.CreatedAt)
        };

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task ClearAndAddAsync(Guid organizationId, string forecastType, IEnumerable<Forecast> newForecasts, CancellationToken cancellationToken = default)
    {
        var existing = await _context.Forecasts
            .Where(f => f.OrganizationId == organizationId && f.ForecastType.ToLower() == forecastType.ToLower())
            .ToListAsync(cancellationToken);
            
        _context.Forecasts.RemoveRange(existing);
        await _context.Forecasts.AddRangeAsync(newForecasts, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
