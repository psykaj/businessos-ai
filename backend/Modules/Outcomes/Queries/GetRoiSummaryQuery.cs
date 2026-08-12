using MediatR;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Modules.Outcomes.DTOs;
using backend.Modules.Outcomes.Entities;

namespace backend.Modules.Outcomes.Queries;

public class GetRoiSummaryQuery : IRequest<RoiSummaryDto>
{
    public Guid BusinessId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal? EstimatedHourlyCost { get; set; }
}

public class GetRoiSummaryQueryHandler : IRequestHandler<GetRoiSummaryQuery, RoiSummaryDto>
{
    private readonly ApplicationDbContext _context;

    public GetRoiSummaryQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RoiSummaryDto> Handle(GetRoiSummaryQuery request, CancellationToken cancellationToken)
    {
        var outcomes = await _context.BusinessOutcomes
            .Where(o => o.BusinessId == request.BusinessId && 
                        o.OccurredAt >= request.StartDate && 
                        o.OccurredAt <= request.EndDate &&
                        o.Status != OutcomeStatus.Voided)
            .ToListAsync(cancellationToken);

        var summary = new RoiSummaryDto
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        if (!outcomes.Any())
        {
            return summary;
        }

        // Calculate totals
        summary.RevenueGenerated = outcomes.Where(o => o.OutcomeType == OutcomeType.RevenueIncrease).Sum(o => o.RevenueImpact ?? 0);
        summary.RevenueRecovered = outcomes.Where(o => o.OutcomeType == OutcomeType.RevenueRecovered).Sum(o => o.RevenueImpact ?? 0);
        summary.CostSaved = outcomes.Sum(o => o.CostImpact ?? 0);
        summary.TimeSavedMinutes = outcomes.Sum(o => o.TimeSavedMinutes ?? 0);
        
        summary.CustomersRetained = outcomes.Where(o => o.OutcomeType == OutcomeType.CustomerRetained).Sum(o => o.CustomerImpact ?? 0);
        summary.CustomersConverted = outcomes.Where(o => o.OutcomeType == OutcomeType.CustomerConverted).Sum(o => o.CustomerImpact ?? 0);
        summary.LeadsConverted = outcomes.Where(o => o.OutcomeType == OutcomeType.LeadConverted).Count();

        summary.SuccessfulActions = outcomes.Count(o => o.SourceType == OutcomeSourceType.ActionCenter);
        summary.SuccessfulAutomations = outcomes.Count(o => o.SourceType == OutcomeSourceType.Automation);

        // Group by Confidence
        summary.ConfidenceSummary = outcomes
            .GroupBy(o => o.Confidence.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        // Calculate Estimated Time Value if hourly rate is provided
        if (request.EstimatedHourlyCost.HasValue && summary.TimeSavedMinutes > 0)
        {
            var hoursSaved = summary.TimeSavedMinutes / 60m;
            summary.EstimatedTimeValue = hoursSaved * request.EstimatedHourlyCost.Value;
        }

        return summary;
    }
}
