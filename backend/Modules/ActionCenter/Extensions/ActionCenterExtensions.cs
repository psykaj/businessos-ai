using Microsoft.Extensions.DependencyInjection;
using backend.Modules.ActionCenter.Repositories;

namespace backend.Modules.ActionCenter.Extensions;

public static class ActionCenterExtensions
{
    public static IServiceCollection AddActionCenterModule(this IServiceCollection services)
    {
        services.AddScoped<IAiActionRepository, AiActionRepository>();
        // Add other domain services here if needed
        return services;
    }
}
