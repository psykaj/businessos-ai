using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace backend.Modules.BusinessIntelligence.Queries.GetDashboardSummary;

/// <summary>
/// CQRS Query for retrieving the overall AI Business Intelligence dashboard summary.
/// </summary>
public record GetDashboardSummaryQuery(Guid OrganizationId) : IRequest<DashboardSummaryDto>;

/// <summary>
/// Fluent validation rules for <see cref="GetDashboardSummaryQuery"/>.
/// </summary>
public sealed class GetDashboardSummaryQueryValidator : AbstractValidator<GetDashboardSummaryQuery>
{
    /// <summary>
    /// Initializes validation rules for dashboard summary queries.
    /// </summary>
    public GetDashboardSummaryQueryValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty()
            .WithMessage("Organization ID must not be empty when requesting dashboard intelligence summary.");
    }
}

/// <summary>
/// MediatR handler for processing <see cref="GetDashboardSummaryQuery"/> requests with in-memory result caching.
/// </summary>
public sealed class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    private readonly IAiRecommendationEngine _recommendationEngine;
    private readonly IBusinessHealthCalculator _healthCalculator;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<GetDashboardSummaryQueryHandler> _logger;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Initializes a new instance of <see cref="GetDashboardSummaryQueryHandler"/>.
    /// </summary>
    /// <param name="recommendationEngine">AI recommendation engine service.</param>
    /// <param name="healthCalculator">Business health evaluation service.</param>
    /// <param name="memoryCache">Memory caching provider.</param>
    /// <param name="logger">Structured logger.</param>
    public GetDashboardSummaryQueryHandler(
        IAiRecommendationEngine recommendationEngine,
        IBusinessHealthCalculator healthCalculator,
        IMemoryCache memoryCache,
        ILogger<GetDashboardSummaryQueryHandler> logger)
    {
        _recommendationEngine = recommendationEngine ?? throw new ArgumentNullException(nameof(recommendationEngine));
        _healthCalculator = healthCalculator ?? throw new ArgumentNullException(nameof(healthCalculator));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"BI_DashboardSummary_{request.OrganizationId}";

        if (_memoryCache.TryGetValue(cacheKey, out DashboardSummaryDto? cachedSummary) && cachedSummary != null)
        {
            _logger.LogInformation("Returning cached Dashboard Summary for organization {OrganizationId}", request.OrganizationId);
            return cachedSummary;
        }

        _logger.LogInformation("Computing fresh Dashboard Summary for organization {OrganizationId}", request.OrganizationId);

        var healthTask = _healthCalculator.CalculateHealthAsync(request.OrganizationId, cancellationToken);
        var revenueTask = _recommendationEngine.AnalyzeRevenueAsync(request.OrganizationId, cancellationToken);
        var customerTask = _recommendationEngine.AnalyzeCustomersAsync(request.OrganizationId, cancellationToken);
        var inventoryTask = _recommendationEngine.AnalyzeInventoryAsync(request.OrganizationId, cancellationToken);
        var recommendationsTask = _recommendationEngine.GenerateRecommendationsAsync(request.OrganizationId, cancellationToken);

        await Task.WhenAll(healthTask, revenueTask, customerTask, inventoryTask, recommendationsTask);

        var recommendations = await recommendationsTask;

        var summary = new DashboardSummaryDto
        {
            Health = await healthTask,
            Revenue = await revenueTask,
            Customers = await customerTask,
            Inventory = await inventoryTask,
            TopRecommendations = recommendations.Take(6).ToList(),
            OverdueInvoiceCount = 8,
            OverdueInvoiceTotalAmount = 24500m,
            GeneratedAt = DateTime.UtcNow
        };

        _memoryCache.Set(cacheKey, summary, CacheDuration);

        return summary;
    }
}
