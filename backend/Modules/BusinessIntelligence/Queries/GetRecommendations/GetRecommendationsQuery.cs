using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace backend.Modules.BusinessIntelligence.Queries.GetRecommendations;

/// <summary>
/// CQRS Query for retrieving AI business performance recommendations.
/// </summary>
public record GetRecommendationsQuery(Guid OrganizationId) : IRequest<List<RecommendationDto>>;

/// <summary>
/// Fluent validation rules for <see cref="GetRecommendationsQuery"/>.
/// </summary>
public sealed class GetRecommendationsQueryValidator : AbstractValidator<GetRecommendationsQuery>
{
    /// <summary>
    /// Initializes validation rules for recommendation queries.
    /// </summary>
    public GetRecommendationsQueryValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty()
            .WithMessage("Organization ID must not be empty when fetching AI recommendations.");
    }
}

/// <summary>
/// MediatR handler for processing <see cref="GetRecommendationsQuery"/> requests with caching.
/// </summary>
public sealed class GetRecommendationsQueryHandler : IRequestHandler<GetRecommendationsQuery, List<RecommendationDto>>
{
    private readonly IAiRecommendationEngine _recommendationEngine;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<GetRecommendationsQueryHandler> _logger;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Initializes a new instance of <see cref="GetRecommendationsQueryHandler"/>.
    /// </summary>
    /// <param name="recommendationEngine">AI recommendation engine service.</param>
    /// <param name="memoryCache">Memory caching provider.</param>
    /// <param name="logger">Structured logger.</param>
    public GetRecommendationsQueryHandler(
        IAiRecommendationEngine recommendationEngine,
        IMemoryCache memoryCache,
        ILogger<GetRecommendationsQueryHandler> logger)
    {
        _recommendationEngine = recommendationEngine ?? throw new ArgumentNullException(nameof(recommendationEngine));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<List<RecommendationDto>> Handle(GetRecommendationsQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"BI_Recommendations_{request.OrganizationId}";

        if (_memoryCache.TryGetValue(cacheKey, out List<RecommendationDto>? cachedData) && cachedData != null)
        {
            _logger.LogInformation("Returning cached Recommendations for organization {OrganizationId}", request.OrganizationId);
            return cachedData;
        }

        _logger.LogInformation("Evaluating fresh AI Recommendations for organization {OrganizationId}", request.OrganizationId);
        var recommendations = await _recommendationEngine.GenerateRecommendationsAsync(request.OrganizationId, cancellationToken);

        _memoryCache.Set(cacheKey, recommendations, CacheDuration);
        return recommendations;
    }
}
