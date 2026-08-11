using Microsoft.Extensions.DependencyInjection;
using backend.Modules.BusinessMemory.Interfaces;
using backend.Modules.BusinessMemory.Services;

namespace backend.Modules.BusinessMemory.Extensions;

public static class BusinessMemoryModuleExtensions
{
    public static IServiceCollection AddBusinessMemoryModule(this IServiceCollection services)
    {
        services.AddScoped<IMemoryRelevanceService, MemoryRelevanceService>();
        services.AddScoped<IMemoryWriter, MemoryWriter>();
        services.AddScoped<IMemoryRetrievalService, MemoryRetrievalService>();
        services.AddScoped<IMemorySummarizer, MemorySummarizer>();
        services.AddScoped<IMemoryService, MemoryService>();

        return services;
    }
}
