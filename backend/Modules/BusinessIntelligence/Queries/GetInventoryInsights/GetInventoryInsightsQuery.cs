using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace backend.Modules.BusinessIntelligence.Queries.GetInventoryInsights;

/// <summary>
/// CQRS Query for retrieving inventory low stock analytics and product velocity insights.
/// </summary>
public record GetInventoryInsightsQuery(Guid OrganizationId) : IRequest<InventoryInsightDto>;

/// <summary>
/// Fluent validation rules for <see cref="GetInventoryInsightsQuery"/>.
/// </summary>
public sealed class GetInventoryInsightsQueryValidator : AbstractValidator<GetInventoryInsightsQuery>
{
    /// <summary>
    /// Initializes validation rules for inventory insight queries.
    /// </summary>
    public GetInventoryInsightsQueryValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty()
            .WithMessage("Organization ID must not be empty when requesting inventory analytics.");
    }
}

/// <summary>
/// MediatR handler for processing <see cref="GetInventoryInsightsQuery"/> requests with caching.
/// </summary>
public sealed class GetInventoryInsightsQueryHandler : IRequestHandler<GetInventoryInsightsQuery, InventoryInsightDto>
{
    private readonly IAiRecommendationEngine _recommendationEngine;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<GetInventoryInsightsQueryHandler> _logger;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Initializes a new instance of <see cref="GetInventoryInsightsQueryHandler"/>.
    /// </summary>
    /// <param name="recommendationEngine">AI recommendation engine service.</param>
    /// <param name="memoryCache">Memory caching provider.</param>
    /// <param name="logger">Structured logger.</param>
    public GetInventoryInsightsQueryHandler(
        IAiRecommendationEngine recommendationEngine,
        IMemoryCache memoryCache,
        ILogger<GetInventoryInsightsQueryHandler> logger)
    {
        _recommendationEngine = recommendationEngine ?? throw new ArgumentNullException(nameof(recommendationEngine));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<InventoryInsightDto> Handle(GetInventoryInsightsQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"BI_InventoryInsights_{request.OrganizationId}";

        if (_memoryCache.TryGetValue(cacheKey, out InventoryInsightDto? cachedInventory) && cachedInventory != null)
        {
            _logger.LogInformation("Returning cached Inventory Insights for organization {OrganizationId}", request.OrganizationId);
            return cachedInventory;
        }

        _logger.LogInformation("Evaluating fresh Inventory Insights for organization {OrganizationId}", request.OrganizationId);
        var inventory = await _recommendationEngine.AnalyzeInventoryAsync(request.OrganizationId, cancellationToken);

        _memoryCache.Set(cacheKey, inventory, CacheDuration);
        return inventory;
    }
}
