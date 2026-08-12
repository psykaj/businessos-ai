using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Modules.Outcomes.Entities;

namespace backend.Modules.Outcomes.Services;

public class OutcomeAttributionService : IOutcomeAttributionService
{
    private readonly ApplicationDbContext _context;

    public OutcomeAttributionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OutcomeConfidence> CalculateConfidenceAsync(Guid businessId, string entityType, string entityId, DateTime eventDate, CancellationToken cancellationToken)
    {
        // This simulates checking recent actions or automations on the entity
        // Real implementation would join AiActions or WorkflowExecutions
        
        // For example, if we have a recorded AI action for this entity within 24 hours:
        // Strong attribution. Within 7 days: Moderate. Otherwise Weak or Unknown.
        
        // We'll query if any past Outcome or Action touches this, but for simplicity of this service:
        // We can check if there was a BusinessMemory or AiAction created recently.
        // We will default to Medium for demonstration, assuming the calling service has basic evidence.

        return Task.FromResult(OutcomeConfidence.Medium).Result;
    }

    public Task<AttributionLevel> DetermineAttributionLevelAsync(OutcomeConfidence confidence, CancellationToken cancellationToken)
    {
        return Task.FromResult(confidence switch
        {
            OutcomeConfidence.High => AttributionLevel.Strong,
            OutcomeConfidence.Medium => AttributionLevel.Moderate,
            OutcomeConfidence.Low => AttributionLevel.Weak,
            _ => AttributionLevel.Unknown
        });
    }
}
