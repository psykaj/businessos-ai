using MediatR;
using backend.Modules.Outcomes.DTOs;
using backend.Modules.Outcomes.Queries;

namespace backend.Modules.Outcomes.Services;

public class RoiCalculationService : IRoiCalculationService
{
    private readonly IMediator _mediator;

    public RoiCalculationService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<RoiSummaryDto> GetRoiSummaryAsync(Guid businessId, DateTime startDate, DateTime endDate, decimal? hourlyCost, CancellationToken cancellationToken)
    {
        var query = new GetRoiSummaryQuery
        {
            BusinessId = businessId,
            StartDate = startDate,
            EndDate = endDate,
            EstimatedHourlyCost = hourlyCost
        };
        
        return await _mediator.Send(query, cancellationToken);
    }
}
