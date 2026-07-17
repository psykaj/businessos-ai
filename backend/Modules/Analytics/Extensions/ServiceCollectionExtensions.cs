using backend.Modules.Analytics.Interfaces;
using backend.Modules.Analytics.Repositories;
using backend.Modules.Analytics.Services;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Modules.Analytics.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAnalyticsModule(this IServiceCollection services)
    {
        services.AddScoped<IScanTrackingService, ScanTrackingService>();
        services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
        return services;
    }
}
