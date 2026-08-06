using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace backend.Modules.BusinessIntelligence.Queries.GetBusinessHealth;

/// <summary>
/// CQRS Query for retrieving the calculated Business Health Score and dimension breakdown.
/// </summary>
public record GetBusinessHealthQuery(Guid OrganizationId) : IRequest<BusinessHealthDto>;

/// <summary>
/// Fluent validation rules for <see cref="GetBusinessHealthQuery"/>.
/// </summary>
public sealed class GetBusinessHealthQueryValidator : AbstractValidator<GetBusinessHealthQuery>
{
    /// <summary>
    /// Initializes validation rules for business health queries.
    /// </summary>
    public GetBusinessHealthQueryValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty()
            .WithMessage("Organization ID must not be empty when evaluating business health score.");
    }
}

/// <summary>
/// MediatR handler for processing <see cref="GetBusinessHealthQuery"/> requests with caching.
/// </summary>
public sealed class GetBusinessHealthQueryHandler : IRequestHandler<GetBusinessHealthQuery, BusinessHealthDto>
{
    private readonly IBusinessHealthCalculator _healthCalculator;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<GetBusinessHealthQueryHandler> _logger;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

    /// <summary>
    /// Initializes a new instance of <see cref="GetBusinessHealthQueryHandler"/>.
    /// </summary>
    /// <param name="healthCalculator">Business health service.</param>
    /// <param name="memoryCache">Memory caching provider.</param>
    /// <param name="logger">Structured logger.</param>
    public GetBusinessHealthQueryHandler(
        IBusinessHealthCalculator healthCalculator,
        IMemoryCache memoryCache,
        ILogger<GetBusinessHealthQueryHandler> logger)
    {
        _healthCalculator = healthCalculator ?? throw new ArgumentNullException(nameof(healthCalculator));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<BusinessHealthDto> Handle(GetBusinessHealthQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"BI_BusinessHealth_{request.OrganizationId}";

        if (_memoryCache.TryGetValue(cacheKey, out BusinessHealthDto? cachedHealth) && cachedHealth != null)
        {
            _logger.LogInformation("Returning cached Business Health for organization {OrganizationId}", request.OrganizationId);
            return cachedHealth;
        }

        _logger.LogInformation("Calculating fresh Business Health Score for organization {OrganizationId}", request.OrganizationId);
        var health = await _healthCalculator.CalculateHealthAsync(request.OrganizationId, cancellationToken);

        _memoryCache.Set(cacheKey, health, CacheDuration);
        return health;
    }
}
