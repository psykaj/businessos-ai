using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.DTOs;

namespace backend.Modules.BusinessIntelligence.Services;

/// <summary>
/// Service interface responsible for generating AI business performance and operational recommendations.
/// </summary>
public interface IAiRecommendationEngine
{
    /// <summary>
    /// Asynchronously analyzes organization data and generates prioritized actionable recommendations.
    /// </summary>
    /// <param name="organizationId">The target organization identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of prioritized recommendations.</returns>
    Task<List<RecommendationDto>> GenerateRecommendationsAsync(Guid organizationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously analyzes revenue momentum, repeat customer attribution, and sales trend drivers.
    /// </summary>
    /// <param name="organizationId">The target organization identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Revenue intelligence insights.</returns>
    Task<RevenueInsightDto> AnalyzeRevenueAsync(Guid organizationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously evaluates customer churn risk, satisfaction scores, and retention opportunities.
    /// </summary>
    /// <param name="organizationId">The target organization identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Customer intelligence insights.</returns>
    Task<CustomerInsightDto> AnalyzeCustomersAsync(Guid organizationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously evaluates inventory stock levels, velocity, fastest sellers, and stockout risks.
    /// </summary>
    /// <param name="organizationId">The target organization identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Inventory intelligence insights.</returns>
    Task<InventoryInsightDto> AnalyzeInventoryAsync(Guid organizationId, CancellationToken cancellationToken = default);
}
