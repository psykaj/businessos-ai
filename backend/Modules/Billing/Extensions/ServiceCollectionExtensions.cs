using backend.Modules.Billing.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Modules.Billing.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBillingModule(this IServiceCollection services)
    {
        services.AddScoped<IBillingService, Services.BillingService>();
        services.AddScoped<IRazorpayService, Services.RazorpayService>();
        
        return services;
    }
}
