using Microsoft.Extensions.DependencyInjection;
using backend.Modules.CustomerFeedback.CustomerSatisfaction.BackgroundServices;
using backend.Modules.CustomerFeedback.CustomerSatisfaction.Interfaces;
using backend.Modules.CustomerFeedback.CustomerSatisfaction.Repositories;
using backend.Modules.CustomerFeedback.CustomerSatisfaction.Services;
using backend.Modules.CustomerFeedback.Feedback.BackgroundServices;
using backend.Modules.CustomerFeedback.Feedback.Interfaces;
using backend.Modules.CustomerFeedback.Feedback.Repositories;
using backend.Modules.CustomerFeedback.Feedback.Services;
using backend.Modules.CustomerFeedback.FeedbackAnalytics.BackgroundServices;
using backend.Modules.CustomerFeedback.FeedbackAnalytics.Interfaces;
using backend.Modules.CustomerFeedback.FeedbackAnalytics.Repositories;
using backend.Modules.CustomerFeedback.FeedbackAnalytics.Services;
using backend.Modules.CustomerFeedback.ImprovementActions.BackgroundServices;
using backend.Modules.CustomerFeedback.ImprovementActions.Interfaces;
using backend.Modules.CustomerFeedback.ImprovementActions.Repositories;
using backend.Modules.CustomerFeedback.ImprovementActions.Services;
using backend.Modules.CustomerFeedback.Ratings.BackgroundServices;
using backend.Modules.CustomerFeedback.Ratings.Interfaces;
using backend.Modules.CustomerFeedback.Ratings.Repositories;
using backend.Modules.CustomerFeedback.Ratings.Services;
using backend.Modules.CustomerFeedback.Sentiment.BackgroundServices;
using backend.Modules.CustomerFeedback.Sentiment.Interfaces;
using backend.Modules.CustomerFeedback.Sentiment.Repositories;
using backend.Modules.CustomerFeedback.Sentiment.Services;
using backend.Modules.CustomerFeedback.ServiceQuality.BackgroundServices;
using backend.Modules.CustomerFeedback.ServiceQuality.Interfaces;
using backend.Modules.CustomerFeedback.ServiceQuality.Repositories;
using backend.Modules.CustomerFeedback.ServiceQuality.Services;
using backend.Modules.CustomerFeedback.Surveys.BackgroundServices;
using backend.Modules.CustomerFeedback.Surveys.Interfaces;
using backend.Modules.CustomerFeedback.Surveys.Repositories;
using backend.Modules.CustomerFeedback.Surveys.Services;

namespace backend.Modules.CustomerFeedback;

public static class CustomerFeedbackExtensions
{
    public static IServiceCollection AddCustomerFeedbackModule(this IServiceCollection services)
    {
        // ─── AI Sentiment Engine Foundation ─────────────────────────────────────────
        services.AddSingleton<ISentimentAnalysisEngine, ProviderIndependentSentimentEngine>();
        services.AddScoped<ISentimentRepository, SentimentRepository>();
        services.AddScoped<ISentimentService, SentimentService>();
        services.AddHostedService<BatchSentimentProcessingWorker>();

        // ─── Customer Feedback & Exporting Engine ───────────────────────────────────
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<IFeedbackService, FeedbackService>();
        services.AddHostedService<FeedbackEscalationWorker>();

        // ─── Ratings & Reviews Engine ───────────────────────────────────────────────
        services.AddScoped<IRatingRepository, RatingRepository>();
        services.AddScoped<IRatingService, RatingService>();
        services.AddHostedService<RatingAggregationWorker>();

        // ─── Survey Platform (CSAT/NPS/CES) ─────────────────────────────────────────
        services.AddScoped<ISurveyRepository, SurveyRepository>();
        services.AddScoped<ISurveyService, SurveyService>();
        services.AddHostedService<SurveyExpirationWorker>();

        // ─── Customer Satisfaction Metrics & Churn Risk ──────────────────────────────
        services.AddScoped<ICsatRepository, CsatRepository>();
        services.AddScoped<ICsatService, CsatService>();
        services.AddHostedService<CsatCalculationWorker>();

        // ─── Service Quality & Support SLAs ──────────────────────────────────────────
        services.AddScoped<IServiceQualityRepository, ServiceQualityRepository>();
        services.AddScoped<IServiceQualityService, ServiceQualityService>();
        services.AddHostedService<ServiceSlaMonitorWorker>();

        // ─── Feedback Trends & Root Cause Analytics ─────────────────────────────────
        services.AddScoped<IFeedbackAnalyticsRepository, FeedbackAnalyticsRepository>();
        services.AddScoped<IFeedbackAnalyticsService, FeedbackAnalyticsService>();
        services.AddHostedService<FeedbackTrendAggregationWorker>();

        // ─── AI Improvement Recommendation Engine ────────────────────────────────────
        services.AddScoped<IImprovementActionRepository, ImprovementActionRepository>();
        services.AddScoped<IImprovementActionService, ImprovementActionService>();
        services.AddHostedService<RecommendationDiscoveryWorker>();

        return services;
    }
}
