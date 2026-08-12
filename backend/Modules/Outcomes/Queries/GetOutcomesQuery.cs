using MediatR;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Modules.Outcomes.DTOs;
using backend.Modules.Outcomes.Entities;

namespace backend.Modules.Outcomes.Queries;

public class GetOutcomesQuery : IRequest<List<BusinessOutcomeDto>>
{
    public Guid BusinessId { get; set; }
    public string? SourceType { get; set; }
    public string? SourceId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

public class GetOutcomesQueryHandler : IRequestHandler<GetOutcomesQuery, List<BusinessOutcomeDto>>
{
    private readonly ApplicationDbContext _context;

    public GetOutcomesQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<BusinessOutcomeDto>> Handle(GetOutcomesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.BusinessOutcomes
            .Where(o => o.BusinessId == request.BusinessId && o.Status != OutcomeStatus.Voided);

        if (!string.IsNullOrEmpty(request.SourceType) && Enum.TryParse<OutcomeSourceType>(request.SourceType, true, out var sourceType))
        {
            query = query.Where(o => o.SourceType == sourceType);
        }

        if (!string.IsNullOrEmpty(request.SourceId))
        {
            query = query.Where(o => o.SourceId == request.SourceId);
        }

        var outcomes = await query
            .OrderByDescending(o => o.OccurredAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return outcomes.Select(outcome => new BusinessOutcomeDto
        {
            Id = outcome.Id,
            BusinessId = outcome.BusinessId,
            OutcomeType = outcome.OutcomeType.ToString(),
            SourceType = outcome.SourceType.ToString(),
            SourceId = outcome.SourceId,
            RelatedEntityType = outcome.RelatedEntityType,
            RelatedEntityId = outcome.RelatedEntityId,
            BeforeValue = outcome.BeforeValue,
            AfterValue = outcome.AfterValue,
            ChangeValue = outcome.ChangeValue,
            ChangePercentage = outcome.ChangePercentage,
            Currency = outcome.Currency,
            TimeSavedMinutes = outcome.TimeSavedMinutes,
            RevenueImpact = outcome.RevenueImpact,
            CostImpact = outcome.CostImpact,
            CustomerImpact = outcome.CustomerImpact,
            Confidence = outcome.Confidence.ToString(),
            AttributionLevel = outcome.AttributionLevel.ToString(),
            MeasurementMethod = outcome.MeasurementMethod,
            OccurredAt = outcome.OccurredAt,
            RecordedAt = outcome.RecordedAt,
            Status = outcome.Status.ToString(),
            Metadata = outcome.Metadata,
            Explanation = outcome.Explanation
        }).ToList();
    }
}
