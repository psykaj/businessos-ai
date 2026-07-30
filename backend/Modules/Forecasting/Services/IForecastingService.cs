using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.Forecasting.Entities;

namespace backend.Modules.Forecasting.Services;

public interface IForecastingService
{
    Task<IEnumerable<Forecast>> GenerateForecastAsync(Guid organizationId, string metricName, int monthsAhead);
}
