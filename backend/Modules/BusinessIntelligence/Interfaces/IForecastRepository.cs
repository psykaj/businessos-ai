using backend.Modules.BusinessIntelligence.Entities;

namespace backend.Modules.BusinessIntelligence.Interfaces;

public interface IForecastRepository
{
    Task<Forecast?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<Forecast>> GetByForecastTypeAsync(Guid organizationId, string forecastType, CancellationToken cancellationToken = default);
    Task<(List<Forecast> Items, int TotalCount)> GetPagedAsync(Guid organizationId, int page, int pageSize, string? forecastType = null, string? sortBy = null, bool sortDescending = false, CancellationToken cancellationToken = default);
    Task ClearAndAddAsync(Guid organizationId, string forecastType, IEnumerable<Forecast> newForecasts, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
