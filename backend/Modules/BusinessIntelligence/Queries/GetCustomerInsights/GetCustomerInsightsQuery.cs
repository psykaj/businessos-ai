using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace backend.Modules.BusinessIntelligence.Queries.GetCustomerInsights;

/// <summary>
/// CQRS Query for retrieving customer retention analytics and churn predictions.
/// </summary>
public record GetCustomerInsightsQuery(Guid OrganizationId) : IRequest<CustomerInsightDto>;

/// <summary>
/// Fluent validation rules for <see cref="GetCustomerInsightsQuery"/>.
/// </summary>
public sealed class GetCustomerInsightsQueryValidator : AbstractValidator<GetCustomerInsightsQuery>
{
    /// <summary>
    /// Initializes validation rules for customer insight queries.
    /// </summary>
    public GetCustomerInsightsQueryValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty()
            .WithMessage("Organization ID must not be empty when requesting customer analytics.");
    }
}

/// <summary>
/// MediatR handler for processing <see cref="GetCustomerInsightsQuery"/> requests with caching.
/// </summary>
public sealed class GetCustomerInsightsQueryHandler : IRequestHandler<GetCustomerInsightsQuery, CustomerInsightDto>
{
    private readonly IAiRecommendationEngine _recommendationEngine;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<GetCustomerInsightsQueryHandler> _logger;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Initializes a new instance of <see cref="GetCustomerInsightsQueryHandler"/>.
    /// </summary>
    /// <param name="recommendationEngine">AI recommendation engine service.</param>
    /// <param name="memoryCache">Memory caching provider.</param>
    /// <param name="logger">Structured logger.</param>
    public GetCustomerInsightsQueryHandler(
        IAiRecommendationEngine recommendationEngine,
        IMemoryCache memoryCache,
        ILogger<GetCustomerInsightsQueryHandler> logger)
    {
        _recommendationEngine = recommendationEngine ?? throw new ArgumentNullException(nameof(recommendationEngine));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CustomerInsightDto> Handle(GetCustomerInsightsQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"BI_CustomerInsights_{request.OrganizationId}";

        if (_memoryCache.TryGetValue(cacheKey, out CustomerInsightDto? cachedCustomers) && cachedCustomers != null)
        {
            _logger.LogInformation("Returning cached Customer Insights for organization {OrganizationId}", request.OrganizationId);
            return cachedCustomers;
        }

        _logger.LogInformation("Evaluating fresh Customer Insights for organization {OrganizationId}", request.OrganizationId);
        var customers = await _recommendationEngine.AnalyzeCustomersAsync(request.OrganizationId, cancellationToken);

        _memoryCache.Set(cacheKey, customers, CacheDuration);
        return customers;
    }
}
