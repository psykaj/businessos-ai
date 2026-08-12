using backend.Modules.Outcomes.DTOs;

namespace backend.Modules.Outcomes.Services;

public interface IRoiCalculationService
{
    Task<RoiSummaryDto> GetRoiSummaryAsync(Guid businessId, DateTime startDate, DateTime endDate, decimal? hourlyCost, CancellationToken cancellationToken);
}
