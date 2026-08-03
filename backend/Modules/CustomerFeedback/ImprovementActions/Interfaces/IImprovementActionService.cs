using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.ImprovementActions.DTOs;

namespace backend.Modules.CustomerFeedback.ImprovementActions.Interfaces;

public interface IImprovementActionRepository
{
    Task<IEnumerable<ImprovementRecommendation>> GetAllAsync(Guid organizationId, string? status = null);
    Task<ImprovementRecommendation?> GetByIdAsync(Guid id, Guid organizationId);
    Task<ImprovementRecommendation> AddAsync(ImprovementRecommendation entity);
    Task UpdateAsync(ImprovementRecommendation entity);
}

public interface IImprovementActionService
{
    Task<IEnumerable<ImprovementRecommendationDto>> GetRecommendationsAsync(Guid organizationId, string? status = null);
    Task<IEnumerable<ImprovementRecommendationDto>> GenerateRecommendationsAsync(Guid organizationId);
    Task<ImprovementRecommendationDto?> UpdateStatusAsync(Guid id, Guid organizationId, string newStatus);
}
