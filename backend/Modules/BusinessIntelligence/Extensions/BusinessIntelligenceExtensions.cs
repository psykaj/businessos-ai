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
        
        // Day 29 - Briefing & Alerts
        services.AddScoped<backend.Modules.BusinessIntelligence.Interfaces.IBusinessInsightAggregator, backend.Modules.BusinessIntelligence.Services.BusinessInsightAggregator>();
        services.AddScoped<backend.Modules.BusinessIntelligence.Interfaces.IInsightPrioritizer, backend.Modules.BusinessIntelligence.Services.InsightPrioritizer>();
        services.AddScoped<backend.Modules.BusinessIntelligence.Interfaces.IBriefingGenerator, backend.Modules.BusinessIntelligence.Services.BriefingGenerator>();
        services.AddScoped<backend.Modules.BusinessIntelligence.Interfaces.IBusinessBriefingService, backend.Modules.BusinessIntelligence.Services.BusinessBriefingService>();
        services.AddScoped<backend.Modules.BusinessIntelligence.Interfaces.IProactiveAlertService, backend.Modules.BusinessIntelligence.Services.ProactiveAlertService>();
        
        services.AddHostedService<backend.Modules.BusinessIntelligence.BackgroundServices.BriefingSchedulerBackgroundService>();

        services.AddValidatorsFromAssembly(typeof(BusinessIntelligenceExtensions).Assembly);

        return services;
    }
}
