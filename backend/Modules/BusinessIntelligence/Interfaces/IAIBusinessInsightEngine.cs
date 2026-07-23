using backend.Modules.BusinessIntelligence.DTOs;

namespace backend.Modules.BusinessIntelligence.Interfaces;

public interface IAIBusinessInsightEngine
{
    Task<GenerateInsightsResponseDto> GenerateInsightsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<InsightDto>> GetInsightsAsync(Guid organizationId, string? category = null, string? priority = null, CancellationToken cancellationToken = default);
}
