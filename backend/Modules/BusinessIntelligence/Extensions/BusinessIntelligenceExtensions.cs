using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Modules.BusinessIntelligence.Repositories;
using backend.Modules.BusinessIntelligence.Services;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Modules.BusinessIntelligence.Extensions;

public static class BusinessIntelligenceExtensions
{
    public static IServiceCollection AddBusinessIntelligenceModule(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IKPIRepository, KPIRepository>();
        services.AddScoped<IGoalRepository, GoalRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IInsightRepository, InsightRepository>();
        services.AddScoped<IForecastRepository, ForecastRepository>();

        // Services
        services.AddScoped<IKPICalculationService, KPICalculationService>();
        services.AddScoped<IExecutiveDashboardService, ExecutiveDashboardService>();
        services.AddScoped<IAIBusinessInsightEngine, AIBusinessInsightEngine>();
        services.AddScoped<IForecastEngineService, ForecastEngineService>();
        services.AddScoped<IReportGeneratorService, ReportGeneratorService>();
        services.AddScoped<IReportExportService, ReportExportService>();
        services.AddScoped<IGoalTrackingService, GoalTrackingService>();

        return services;
    }
}
