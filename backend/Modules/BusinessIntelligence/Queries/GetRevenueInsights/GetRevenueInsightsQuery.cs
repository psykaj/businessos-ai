using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace backend.Modules.BusinessIntelligence.Queries.GetRevenueInsights;

/// <summary>
/// CQRS Query for retrieving revenue intelligence and historical momentum analytics.
/// </summary>
public record GetRevenueInsightsQuery(Guid OrganizationId) : IRequest<RevenueInsightDto>;

/// <summary>
/// Fluent validation rules for <see cref="GetRevenueInsightsQuery"/>.
/// </summary>
public sealed class GetRevenueInsightsQueryValidator : AbstractValidator<GetRevenueInsightsQuery>
{
    /// <summary>
    /// Initializes validation rules for revenue insight queries.
    /// </summary>
    public GetRevenueInsightsQueryValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty()
            .WithMessage("Organization ID must not be empty when requesting revenue analytics.");
    }
}

/// <summary>
/// MediatR handler for processing <see cref="GetRevenueInsightsQuery"/> requests with caching.
/// </summary>
public sealed class GetRevenueInsightsQueryHandler : IRequestHandler<GetRevenueInsightsQuery, RevenueInsightDto>
{
    private readonly IAiRecommendationEngine _recommendationEngine;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<GetRevenueInsightsQueryHandler> _logger;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Initializes a new instance of <see cref="GetRevenueInsightsQueryHandler"/>.
    /// </summary>
    /// <param name="recommendationEngine">AI recommendation engine service.</param>
    /// <param name="memoryCache">Memory caching provider.</param>
    /// <param name="logger">Structured logger.</param>
    public GetRevenueInsightsQueryHandler(
        IAiRecommendationEngine recommendationEngine,
        IMemoryCache memoryCache,
        ILogger<GetRevenueInsightsQueryHandler> logger)
    {
        _recommendationEngine = recommendationEngine ?? throw new ArgumentNullException(nameof(recommendationEngine));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<RevenueInsightDto> Handle(GetRevenueInsightsQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"BI_RevenueInsights_{request.OrganizationId}";

        if (_memoryCache.TryGetValue(cacheKey, out RevenueInsightDto? cachedRevenue) && cachedRevenue != null)
        {
            _logger.LogInformation("Returning cached Revenue Insights for organization {OrganizationId}", request.OrganizationId);
            return cachedRevenue;
        }

        _logger.LogInformation("Evaluating fresh Revenue Insights for organization {OrganizationId}", request.OrganizationId);
        var revenue = await _recommendationEngine.AnalyzeRevenueAsync(request.OrganizationId, cancellationToken);

        _memoryCache.Set(cacheKey, revenue, CacheDuration);
        return revenue;
    }
}
