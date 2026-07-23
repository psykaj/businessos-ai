using backend.Modules.BusinessIntelligence.DTOs;

namespace backend.Modules.BusinessIntelligence.Interfaces;

public interface IForecastEngineService
{
    Task<ForecastSummaryDto> GenerateForecastAsync(Guid organizationId, GenerateForecastRequestDto request, CancellationToken cancellationToken = default);
    Task<ForecastSummaryDto> GetForecastAsync(Guid organizationId, string forecastType, CancellationToken cancellationToken = default);
}
