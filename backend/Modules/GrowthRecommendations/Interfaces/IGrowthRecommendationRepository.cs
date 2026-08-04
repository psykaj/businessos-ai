using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.GrowthRecommendations.Entities;

namespace backend.Modules.GrowthRecommendations.Interfaces;

public interface IGrowthRecommendationRepository
{
    Task<GrowthRecommendation?> GetByIdAsync(Guid id, Guid organizationId);
    Task<PagedResult<GrowthRecommendation>> GetRecommendationsAsync(Guid organizationId, string? status, string? priority, string? category, int pageNumber, int pageSize);
    Task<List<GrowthRecommendation>> GetActiveRecommendationsAsync(Guid organizationId);
    Task<GrowthRecommendation> CreateAsync(GrowthRecommendation recommendation);
    Task<GrowthRecommendation> UpdateAsync(GrowthRecommendation recommendation);
    Task<bool> DeleteAsync(Guid id, Guid organizationId);
}
