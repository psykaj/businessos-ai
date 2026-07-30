using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.Forecasting.Entities;
using backend.Modules.Forecasting.Repositories;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Modules.Forecasting.Services;

public class ForecastingService : IForecastingService
{
    private readonly ApplicationDbContext _context;
    private readonly IForecastRepository _forecastRepository;
    private readonly ILogger<ForecastingService> _logger;

    public ForecastingService(ApplicationDbContext context, IForecastRepository forecastRepository, ILogger<ForecastingService> logger)
    {
        _context = context;
        _forecastRepository = forecastRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Forecast>> GenerateForecastAsync(Guid organizationId, string metricName, int monthsAhead)
    {
        // 1. Delete old forecasts for this metric
        await _forecastRepository.DeleteOldForecastsAsync(metricName, organizationId);

        // 2. Mock AI forecasting logic (this would be replaced with actual ML.NET or API calls to AI models)
        decimal baseValue = 0;
        
        switch (metricName.ToLower())
        {
            case "revenue":
                var currentRevenue = await _context.Invoices
                    .Where(i => i.OrganizationId == organizationId && i.Status == "Paid" && i.CreatedAt >= DateTime.UtcNow.AddMonths(-3))
                    .SumAsync(i => i.Amount);
                baseValue = currentRevenue > 0 ? currentRevenue : 10000m;
                break;
            default:
                baseValue = 5000m;
                break;
        }

        var forecasts = new List<Forecast>();
        var random = new Random();

        for (int i = 1; i <= monthsAhead; i++)
        {
            var growthFactor = 1m + (decimal)(random.NextDouble() * 0.1 - 0.02); // -2% to +8% growth
            var predictedValue = baseValue * (decimal)Math.Pow((double)growthFactor, i);
            var variance = predictedValue * 0.1m; // 10% variance

            forecasts.Add(new Forecast
            {
                OrganizationId = organizationId,
                MetricName = metricName,
                PredictedValue = predictedValue,
                LowerBound = predictedValue - variance,
                UpperBound = predictedValue + variance,
                ConfidenceLevel = 85m - (i * 2), // Confidence drops slightly over time
                ForecastDate = DateTime.UtcNow.AddMonths(i),
                ModelUsed = "AI_ARIMA_V1",
                Factors = "{\"seasonality\": \"high\", \"trend\": \"upward\"}"
            });
        }

        await _forecastRepository.AddRangeAsync(forecasts);
        _logger.LogInformation("Generated {MonthsAhead} months forecast for {MetricName} (Org: {OrgId})", monthsAhead, metricName, organizationId);
        
        return forecasts;
    }
}
