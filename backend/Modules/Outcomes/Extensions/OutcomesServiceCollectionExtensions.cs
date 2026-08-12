using Microsoft.Extensions.DependencyInjection;
using backend.Modules.Outcomes.Services;

namespace backend.Modules.Outcomes.Extensions;

public static class OutcomesServiceCollectionExtensions
{
    public static IServiceCollection AddOutcomesModule(this IServiceCollection services)
    {
        services.AddScoped<IOutcomeAttributionService, OutcomeAttributionService>();
        services.AddScoped<IOutcomeDetectionService, OutcomeDetectionService>();
        services.AddScoped<IRoiCalculationService, RoiCalculationService>();
        
        return services;
    }
}
