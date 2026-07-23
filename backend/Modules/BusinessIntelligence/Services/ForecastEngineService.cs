using backend.Modules.BusinessIntelligence.Constants;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Entities;
using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BusinessIntelligence.Services;

public class ForecastEngineService : IForecastEngineService
{
    private readonly ApplicationDbContext _context;
    private readonly IForecastRepository _forecastRepository;

    public ForecastEngineService(ApplicationDbContext context, IForecastRepository forecastRepository)
    {
        _context = context;
        _forecastRepository = forecastRepository;
    }

    public async Task<ForecastSummaryDto> GenerateForecastAsync(Guid organizationId, GenerateForecastRequestDto request, CancellationToken cancellationToken = default)
    {
        var forecastType = string.IsNullOrWhiteSpace(request.ForecastType) ? BIConstants.ForecastTypes.Revenue : request.ForecastType;
        var days = request.HorizonDays > 0 ? request.HorizonDays : 30;

        decimal baseValue = 1000m;
        double growthFactor = 1.05;
        double confidence = 0.88;

        var now = DateTime.UtcNow;

        switch (forecastType.ToLower())
        {
            case "revenue":
                var currentRevenue = await _context.Invoices
                    .Where(i => i.OrganizationId == organizationId && i.Status == "Paid" && i.CreatedAt >= now.AddDays(-30))
                    .SumAsync(i => (decimal?)i.Amount, cancellationToken) ?? 25000m;
                baseValue = currentRevenue > 0 ? currentRevenue / 30m : 850m;
                growthFactor = 1.08;
                confidence = 0.90;
                break;

            case "sales":
                var currentWonValue = await _context.Deals
                    .Where(d => d.OrganizationId == organizationId && d.Status == "Won" && d.UpdatedAt >= now.AddDays(-30))
                    .SumAsync(d => (decimal?)d.Amount, cancellationToken) ?? 18000m;
                baseValue = currentWonValue > 0 ? currentWonValue / 30m : 600m;
                growthFactor = 1.06;
                confidence = 0.85;
                break;

            case "lead":
                var leads30Days = await _context.Leads
                    .CountAsync(l => l.OrganizationId == organizationId && l.CreatedAt >= now.AddDays(-30), cancellationToken);
                baseValue = leads30Days > 0 ? leads30Days / 30m : 12m;
                growthFactor = 1.10;
                confidence = 0.87;
                break;

            case "customergrowth":
                var cust30Days = await _context.Customers
                    .CountAsync(c => c.OrganizationId == organizationId && c.CreatedAt >= now.AddDays(-30), cancellationToken);
                baseValue = cust30Days > 0 ? cust30Days / 30m : 3m;
                growthFactor = 1.04;
                confidence = 0.92;
                break;

            case "subscription":
                var subCount = await _context.Subscriptions
                    .CountAsync(s => s.OrganizationId == organizationId && s.Status == "Active", cancellationToken);
                var subValue = subCount * 299m;
                baseValue = subValue > 0 ? subValue / 30m : 160m;
                growthFactor = 1.03;
                confidence = 0.94;
                break;
        }

        var newForecasts = new List<Forecast>();
        decimal runningBase = baseValue;

        for (int i = 1; i <= days; i++)
        {
            var date = now.Date.AddDays(i);
            var dailyMultiplier = (decimal)(1.0 + (Math.Sin(i * 0.5) * 0.05));
            var predictedDaily = Math.Round(runningBase * dailyMultiplier, 2);

            newForecasts.Add(new Forecast
            {
                OrganizationId = organizationId,
                ForecastType = forecastType,
                PredictedValue = predictedDaily,
                ConfidenceScore = Math.Round(confidence - (i * 0.002), 3),
                ForecastDate = date
            });

            if (i % 7 == 0)
            {
                runningBase *= (decimal)(growthFactor - 1.0 + 1.0);
            }
        }

        await _forecastRepository.ClearAndAddAsync(organizationId, forecastType, newForecasts, cancellationToken);
        await _forecastRepository.SaveChangesAsync(cancellationToken);

        var dataPoints = newForecasts.Select(f => new ForecastDto
        {
            Id = f.Id,
            OrganizationId = f.OrganizationId,
            ForecastType = f.ForecastType,
            PredictedValue = f.PredictedValue,
            ConfidenceScore = f.ConfidenceScore,
            ForecastDate = f.ForecastDate,
            CreatedAt = f.CreatedAt
        }).ToList();

        var totalPredicted = dataPoints.Sum(d => d.PredictedValue);
        var growthRate = (decimal)((growthFactor - 1.0) * 100.0);

        return new ForecastSummaryDto
        {
            ForecastType = forecastType,
            TotalPredicted = totalPredicted,
            GrowthRate = (decimal)Math.Round(growthRate, 2),
            AverageConfidence = Math.Round(dataPoints.Average(d => d.ConfidenceScore), 2),
            DataPoints = dataPoints
        };
    }

    public async Task<ForecastSummaryDto> GetForecastAsync(Guid organizationId, string forecastType, CancellationToken cancellationToken = default)
    {
        var existing = await _forecastRepository.GetByForecastTypeAsync(organizationId, forecastType, cancellationToken);

        if (existing.Count == 0)
        {
            return await GenerateForecastAsync(organizationId, new GenerateForecastRequestDto { ForecastType = forecastType, HorizonDays = 30 }, cancellationToken);
        }

        var dataPoints = existing.Select(f => new ForecastDto
        {
            Id = f.Id,
            OrganizationId = f.OrganizationId,
            ForecastType = f.ForecastType,
            PredictedValue = f.PredictedValue,
            ConfidenceScore = f.ConfidenceScore,
            ForecastDate = f.ForecastDate,
            CreatedAt = f.CreatedAt
        }).ToList();

        return new ForecastSummaryDto
        {
            ForecastType = forecastType,
            TotalPredicted = dataPoints.Sum(d => d.PredictedValue),
            GrowthRate = 6.5m,
            AverageConfidence = Math.Round(dataPoints.Average(d => d.ConfidenceScore), 2),
            DataPoints = dataPoints
        };
    }
}
