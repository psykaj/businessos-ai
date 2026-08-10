using Microsoft.Extensions.DependencyInjection;
using backend.Modules.CommandCenter.Interfaces;
using backend.Modules.CommandCenter.Services;

namespace backend.Modules.CommandCenter.Extensions;

public static class CommandCenterExtensions
{
    public static IServiceCollection AddCommandCenterModule(this IServiceCollection services)
    {
        services.AddScoped<ICommandCenterService, CommandCenterService>();
        return services;
    }
}
