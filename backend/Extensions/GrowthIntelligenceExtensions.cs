using Microsoft.Extensions.DependencyInjection;
using backend.Modules.Benchmarking.BackgroundJobs;
using backend.Modules.Benchmarking.Interfaces;
using backend.Modules.Benchmarking.Repositories;
using backend.Modules.Benchmarking.Services;
using backend.Modules.BusinessPerformance.BackgroundJobs;
using backend.Modules.BusinessPerformance.Interfaces;
using backend.Modules.BusinessPerformance.Repositories;
using backend.Modules.BusinessPerformance.Services;
using backend.Modules.CustomerAnalytics.BackgroundJobs;
using backend.Modules.CustomerAnalytics.Interfaces;
using backend.Modules.CustomerAnalytics.Repositories;
using backend.Modules.CustomerAnalytics.Services;
using backend.Modules.GrowthCenter.BackgroundJobs;
using backend.Modules.GrowthCenter.Interfaces;
using backend.Modules.GrowthCenter.Services;
using backend.Modules.GrowthRecommendations.BackgroundJobs;
using backend.Modules.GrowthRecommendations.Interfaces;
using backend.Modules.GrowthRecommendations.Repositories;
using backend.Modules.GrowthRecommendations.Services;
using backend.Modules.MarketingROI.BackgroundJobs;
using backend.Modules.MarketingROI.Interfaces;
using backend.Modules.MarketingROI.Repositories;
using backend.Modules.MarketingROI.Services;
using backend.Modules.ProductAnalytics.BackgroundJobs;
using backend.Modules.ProductAnalytics.Interfaces;
using backend.Modules.ProductAnalytics.Repositories;
using backend.Modules.ProductAnalytics.Services;
using backend.Modules.Profitability.BackgroundJobs;
using backend.Modules.Profitability.Interfaces;
using backend.Modules.Profitability.Repositories;
using backend.Modules.Profitability.Services;
using backend.Modules.RevenueAnalytics.BackgroundJobs;
using backend.Modules.RevenueAnalytics.Interfaces;
using backend.Modules.RevenueAnalytics.Repositories;
using backend.Modules.RevenueAnalytics.Services;

namespace backend.Extensions;

public static class GrowthIntelligenceExtensions
{
    public static IServiceCollection AddGrowthIntelligenceModule(this IServiceCollection services)
    {
        // ─── Module 1: Business Performance Engine ─────────────────────────────────
        services.AddScoped<IBusinessPerformanceRepository, BusinessPerformanceRepository>();
        services.AddScoped<IBusinessPerformanceService, BusinessPerformanceService>();
        services.AddHostedService<BusinessPerformanceCalculationJob>();

        // ─── Module 2: Revenue Analytics ───────────────────────────────────────────
        services.AddScoped<IRevenueAnalyticsRepository, RevenueAnalyticsRepository>();
        services.AddScoped<IRevenueAnalyticsService, RevenueAnalyticsService>();
        services.AddHostedService<RevenueSnapshotJob>();

        // ─── Module 3: Profitability Analysis & Loss Center Detection ──────────────
        services.AddScoped<IProfitabilityRepository, ProfitabilityRepository>();
        services.AddScoped<IProfitabilityService, ProfitabilityService>();
        services.AddHostedService<ProfitabilityAnalysisJob>();

        // ─── Module 4: Product Analytics & Margin Ranking ──────────────────────────
        services.AddScoped<IProductAnalyticsRepository, ProductAnalyticsRepository>();
        services.AddScoped<IProductAnalyticsService, ProductAnalyticsService>();
        services.AddHostedService<ProductRankingWorkerJob>();

        // ─── Module 5: Customer Analytics & LTV/CAC Engine ─────────────────────────
        services.AddScoped<ICustomerAnalyticsRepository, CustomerAnalyticsRepository>();
        services.AddScoped<ICustomerAnalyticsService, CustomerAnalyticsService>();
        services.AddHostedService<CustomerLtvCacWorkerJob>();

        // ─── Module 6: Marketing ROI & Budget Optimization ─────────────────────────
        services.AddScoped<IMarketingRoiRepository, MarketingRoiRepository>();
        services.AddScoped<IMarketingRoiService, MarketingRoiService>();
        services.AddHostedService<MarketingChannelOptimizerJob>();

        // ─── Module 7: AI Growth Recommendations Engine ─────────────────────────────
        services.AddScoped<IGrowthRecommendationRepository, GrowthRecommendationRepository>();
        services.AddScoped<IGrowthRecommendationService, GrowthRecommendationService>();
        services.AddHostedService<RecommendationGeneratorJob>();

        // ─── Module 8: Industry Benchmarking ────────────────────────────────────────
        services.AddScoped<IBenchmarkRepository, BenchmarkRepository>();
        services.AddScoped<IBenchmarkService, BenchmarkService>();
        services.AddHostedService<IndustryBenchmarkSyncJob>();

        // ─── Module 9: AI Growth Center Master Orchestration ────────────────────────
        services.AddScoped<IGrowthCenterService, GrowthCenterService>();
        services.AddHostedService<GrowthCenterOrchestrationJob>();

        return services;
    }
}
