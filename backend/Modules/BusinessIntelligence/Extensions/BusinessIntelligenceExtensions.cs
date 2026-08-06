using System.Reflection;
using backend.Modules.BusinessIntelligence.Repositories;
using backend.Modules.BusinessIntelligence.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Modules.BusinessIntelligence.Extensions;

/// <summary>
/// Extension methods for registering the AI Business Intelligence engine module and its dependencies in the DI container.
/// </summary>
public static class BusinessIntelligenceExtensions
{
    /// <summary>
    /// Registers repositories, services, CQRS handlers, and validators for the Business Intelligence module.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddBusinessIntelligenceModule(this IServiceCollection services)
    {
        services.AddScoped<IBusinessIntelligenceRepository, BusinessIntelligenceRepository>();
        services.AddScoped<IAiRecommendationEngine, AiRecommendationEngine>();
        services.AddScoped<IBusinessHealthCalculator, BusinessHealthCalculator>();

        services.AddValidatorsFromAssembly(typeof(BusinessIntelligenceExtensions).Assembly);

        return services;
    }
}
