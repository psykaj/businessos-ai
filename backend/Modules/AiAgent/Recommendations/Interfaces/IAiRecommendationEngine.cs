using backend.Modules.AiAgent.Entities;

namespace backend.Modules.AiAgent.Recommendations.Interfaces;

public interface IAiRecommendationEngine
{
    Task<List<Recommendation>> GenerateRecommendationsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<Recommendation?> ApplyRecommendationAsync(Guid recommendationId, Guid organizationId, CancellationToken cancellationToken = default);
}
