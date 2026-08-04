using System;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.GrowthRecommendations.DTOs;

namespace backend.Modules.GrowthRecommendations.Interfaces;

public interface IGrowthRecommendationService
{
    Task<GrowthRecommendationSummaryDto> GetSummaryAsync(Guid organizationId);
    Task<PagedResult<GrowthRecommendationDto>> GetRecommendationsAsync(Guid organizationId, string? status, string? priority, string? category, int pageNumber, int pageSize);
    Task<GrowthRecommendationDto> CreateRecommendationAsync(Guid organizationId, CreateGrowthRecommendationRequest request, string? userId = null);
    Task<GrowthRecommendationDto> UpdateStatusAsync(Guid id, Guid organizationId, UpdateRecommendationStatusRequest request, string? userId = null);
    Task<GrowthRecommendationSummaryDto> GenerateAiRecommendationsAsync(Guid organizationId);
    Task<bool> DeleteRecommendationAsync(Guid id, Guid organizationId);
    Task<string> ExportRecommendationsReportAsync(Guid organizationId);
}
