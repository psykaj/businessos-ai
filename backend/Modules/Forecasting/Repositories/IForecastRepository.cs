using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.Forecasting.Entities;

namespace backend.Modules.Forecasting.Repositories;

public interface IForecastRepository
{
    Task<IEnumerable<Forecast>> GetAllAsync(Guid organizationId);
    Task<IEnumerable<Forecast>> GetByMetricAsync(string metricName, Guid organizationId);
    Task<Forecast> AddAsync(Forecast forecast);
    Task AddRangeAsync(IEnumerable<Forecast> forecasts);
    Task DeleteOldForecastsAsync(string metricName, Guid organizationId);
}
