using backend.Modules.DailyOperatingLoop.Interfaces;
using backend.Modules.DailyOperatingLoop.Services;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Modules.DailyOperatingLoop.Extensions;

public static class DailyOperatingLoopExtensions
{
    public static IServiceCollection AddDailyOperatingLoopModule(this IServiceCollection services)
    {
        services.AddScoped<IDailyPriorityScoringService, DailyPriorityScoringService>();
        services.AddScoped<IDailyBriefingService, DailyBriefingService>();
        services.AddScoped<IDailyPriorityService, DailyPriorityService>();
        
        return services;
    }
}
