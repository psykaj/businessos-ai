using backend.Modules.KpiEngine.Repositories;
using backend.Modules.KpiEngine.Services;
using backend.Modules.KpiEngine.Jobs;
using backend.Modules.Forecasting.Repositories;
using backend.Modules.Forecasting.Services;
using backend.Modules.ExecutiveInsights.Repositories;
using backend.Modules.ExecutiveInsights.Services;
using backend.Modules.BusinessGoals.Repositories;
using backend.Modules.BusinessGoals.Services;
using backend.Modules.Scorecards.Repositories;
using backend.Modules.Scorecards.Services;
using backend.Modules.BusinessHealth.Repositories;
using backend.Modules.BusinessHealth.Services;
using backend.Modules.Benchmarks.Repositories;
using backend.Modules.AiRecommendations.Repositories;
using backend.Modules.DecisionCenter.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Modules.ExecutiveInsights.Extensions;

public static class ExecutiveIntelligenceExtensions
{
    public static IServiceCollection AddExecutiveIntelligenceModule(this IServiceCollection services)
    {
        // KPI Engine
        services.AddScoped<IKpiRepository, KpiRepository>();
        services.AddScoped<IKpiCalculationService, KpiCalculationService>();
        services.AddHostedService<KpiCalculationBackgroundJob>();

        // Forecasting
        services.AddScoped<IForecastRepository, ForecastRepository>();
        services.AddScoped<IForecastingService, ForecastingService>();

        // Executive Insights
        services.AddScoped<IExecutiveInsightRepository, ExecutiveInsightRepository>();
        services.AddScoped<IInsightGenerationService, InsightGenerationService>();

        // Business Goals
        services.AddScoped<IBusinessGoalRepository, BusinessGoalRepository>();
        services.AddScoped<GoalTrackingService>();
        services.AddScoped<IGoalProjectionService, GoalProjectionService>();
        services.AddScoped<IGoalGapAnalysisService, GoalGapAnalysisService>();
        services.AddScoped<IGoalCoachService, GoalCoachService>();

        // Scorecards
        services.AddScoped<IScorecardRepository, ScorecardRepository>();
        services.AddScoped<ScorecardService>();

        // Business Health
        services.AddScoped<IBusinessHealthRepository, BusinessHealthRepository>();
        services.AddScoped<BusinessHealthService>();

        // Benchmarks
        services.AddScoped<IBenchmarkRepository, BenchmarkRepository>();

        // AI Recommendations
        services.AddScoped<IAiRecommendationRepository, AiRecommendationRepository>();

        // Decision Center
        services.AddScoped<IDecisionLogRepository, DecisionLogRepository>();

        return services;
    }
}
