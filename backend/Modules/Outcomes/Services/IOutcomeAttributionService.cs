using backend.Modules.Outcomes.Entities;

namespace backend.Modules.Outcomes.Services;

public interface IOutcomeAttributionService
{
    Task<OutcomeConfidence> CalculateConfidenceAsync(Guid businessId, string entityType, string entityId, DateTime eventDate, CancellationToken cancellationToken);
    
    Task<AttributionLevel> DetermineAttributionLevelAsync(OutcomeConfidence confidence, CancellationToken cancellationToken);
}
