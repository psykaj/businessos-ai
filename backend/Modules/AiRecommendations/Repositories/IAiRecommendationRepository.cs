using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.AiRecommendations.Entities;

namespace backend.Modules.AiRecommendations.Repositories;

public interface IAiRecommendationRepository
{
    Task<IEnumerable<AiRecommendation>> GetAllAsync(Guid organizationId);
    Task<IEnumerable<AiRecommendation>> GetPendingAsync(Guid organizationId);
    Task<AiRecommendation?> GetByIdAsync(Guid id, Guid organizationId);
    Task<AiRecommendation> AddAsync(AiRecommendation recommendation);
    Task UpdateAsync(AiRecommendation recommendation);
}
