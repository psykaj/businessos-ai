using MediatR;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Modules.Outcomes.Entities;
using backend.Modules.Outcomes.DTOs;
using backend.Common; // For user context if available, otherwise assume standard exceptions
using backend.Exceptions; // Assuming NotFoundException or ValidationException exists

namespace backend.Modules.Outcomes.Commands;

public class CreateOutcomeCommand : IRequest<BusinessOutcomeDto>
{
    public Guid BusinessId { get; set; }
    public CreateOutcomeDto Data { get; set; } = new();
}

public class CreateOutcomeCommandHandler : IRequestHandler<CreateOutcomeCommand, BusinessOutcomeDto>
{
    private readonly ApplicationDbContext _context;
    
    public CreateOutcomeCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BusinessOutcomeDto> Handle(CreateOutcomeCommand request, CancellationToken cancellationToken)
    {
        // Parse enums safely
        if (!Enum.TryParse<OutcomeType>(request.Data.OutcomeType, true, out var outcomeType))
        {
            throw new BadRequestException($"Invalid OutcomeType: {request.Data.OutcomeType}");
        }

        if (!Enum.TryParse<OutcomeSourceType>(request.Data.SourceType, true, out var sourceType))
        {
            throw new BadRequestException($"Invalid SourceType: {request.Data.SourceType}");
        }

        if (!Enum.TryParse<OutcomeConfidence>(request.Data.Confidence, true, out var confidence))
        {
            throw new BadRequestException($"Invalid Confidence: {request.Data.Confidence}");
        }

        // Deduplication check
        var exists = await _context.BusinessOutcomes
            .AnyAsync(o => o.BusinessId == request.BusinessId && 
                           o.SourceType == sourceType && 
                           o.SourceId == request.Data.SourceId && 
                           o.OutcomeType == outcomeType, 
                           cancellationToken);

        if (exists)
        {
            throw new BadRequestException("Duplicate outcome detected for this source and type.");
        }

        var outcome = new BusinessOutcome
        {
            BusinessId = request.BusinessId,
            OutcomeType = outcomeType,
            SourceType = sourceType,
            SourceId = request.Data.SourceId,
            RelatedEntityType = request.Data.RelatedEntityType,
            RelatedEntityId = request.Data.RelatedEntityId,
            BeforeValue = request.Data.BeforeValue,
            AfterValue = request.Data.AfterValue,
            ChangeValue = request.Data.ChangeValue,
            ChangePercentage = request.Data.ChangePercentage,
            Currency = request.Data.Currency,
            TimeSavedMinutes = request.Data.TimeSavedMinutes,
            RevenueImpact = request.Data.RevenueImpact,
            CostImpact = request.Data.CostImpact,
            CustomerImpact = request.Data.CustomerImpact,
            Confidence = confidence,
            MeasurementMethod = request.Data.MeasurementMethod,
            Metadata = request.Data.Metadata,
            Explanation = request.Data.Explanation,
            OccurredAt = request.Data.OccurredAt ?? DateTime.UtcNow,
            RecordedAt = DateTime.UtcNow,
            Status = OutcomeStatus.Active,
            AttributionLevel = AttributionLevel.Unknown // Can be updated by a service later or passed in
        };

        _context.BusinessOutcomes.Add(outcome);
        await _context.SaveChangesAsync(cancellationToken);

        return new BusinessOutcomeDto
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
        };
    }
}
