using backend.Modules.CustomerSuccess.CustomerHealth.Interfaces;
using backend.Modules.CustomerSuccess.CustomerHealth.Repositories;
using backend.Modules.CustomerSuccess.CustomerHealth.Services;
using backend.Modules.CustomerSuccess.CustomerSegments.Interfaces;
using backend.Modules.CustomerSuccess.CustomerSegments.Repositories;
using backend.Modules.CustomerSuccess.CustomerSegments.Services;
using backend.Modules.CustomerSuccess.Loyalty.Interfaces;
using backend.Modules.CustomerSuccess.Loyalty.Repositories;
using backend.Modules.CustomerSuccess.Loyalty.Services;
using backend.Modules.CustomerSuccess.Referrals.Interfaces;
using backend.Modules.CustomerSuccess.Referrals.Repositories;
using backend.Modules.CustomerSuccess.Referrals.Services;
using backend.Modules.CustomerSuccess.Retention.Interfaces;
using backend.Modules.CustomerSuccess.Retention.Services;
using backend.Modules.CustomerSuccess.Satisfaction.Interfaces;
using backend.Modules.CustomerSuccess.Satisfaction.Repositories;
using backend.Modules.CustomerSuccess.Satisfaction.Services;
using backend.Modules.CustomerSuccess.SuccessTasks.Interfaces;
using backend.Modules.CustomerSuccess.SuccessTasks.Repositories;
using backend.Modules.CustomerSuccess.SuccessTasks.Services;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Modules.CustomerSuccess.Extensions;

public static class CustomerSuccessExtensions
{
    public static IServiceCollection AddCustomerSuccessModule(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<ICustomerHealthRepository, CustomerHealthRepository>();
        services.AddScoped<ILoyaltyRepository, LoyaltyRepository>();
        services.AddScoped<IReferralRepository, ReferralRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<ISuccessTaskRepository, SuccessTaskRepository>();
        services.AddScoped<ICustomerSegmentRepository, CustomerSegmentRepository>();

        // Services
        services.AddScoped<ICustomerHealthService, CustomerHealthService>();
        services.AddScoped<ILoyaltyService, LoyaltyService>();
        services.AddScoped<IReferralService, ReferralService>();
        services.AddScoped<ISatisfactionService, SatisfactionService>();
        services.AddScoped<ISuccessTaskService, SuccessTaskService>();
        services.AddScoped<ICustomerSegmentService, CustomerSegmentService>();
        services.AddScoped<IRetentionService, RetentionService>();

        return services;
    }
}
