using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Modules.KpiEngine.Repositories;
using backend.Modules.Scorecards.Entities;
using backend.Modules.Scorecards.Repositories;

namespace backend.Modules.Scorecards.Services;

public class ScorecardService
{
    private readonly IScorecardRepository _scorecardRepository;
    private readonly IKpiRepository _kpiRepository;

    public ScorecardService(IScorecardRepository scorecardRepository, IKpiRepository kpiRepository)
    {
        _scorecardRepository = scorecardRepository;
        _kpiRepository = kpiRepository;
    }

    public async Task GenerateScorecardAsync(Guid organizationId, string name, string department, DateTime periodStart, DateTime periodEnd)
    {
        // 1. Fetch relevant KPIs for the department
        var allKpis = await _kpiRepository.GetAllAsync(organizationId);
        var relevantKpis = allKpis.Where(k => string.IsNullOrEmpty(department) || k.Category.Contains(department, StringComparison.OrdinalIgnoreCase)).ToList();

        // 2. Calculate overall score based on target vs current value
        decimal totalScore = 0;
        int count = 0;

        var summary = relevantKpis.Select(k =>
        {
            var kpiScore = k.TargetValue > 0 ? (k.CurrentValue / k.TargetValue) * 100 : 0;
            if (kpiScore > 100) kpiScore = 100;
            
            totalScore += kpiScore;
            count++;

            return new
            {
                KpiId = k.Id,
                Name = k.Name,
                CurrentValue = k.CurrentValue,
                TargetValue = k.TargetValue,
                Score = kpiScore
            };
        }).ToList();

        decimal overallScore = count > 0 ? totalScore / count : 0;

        var scorecard = new Scorecard
        {
            OrganizationId = organizationId,
            Name = name,
            Description = $"Auto-generated scorecard for {department}",
            Department = department,
            PeriodStart = periodStart,
            PeriodEnd = periodEnd,
            OverallScore = overallScore,
            KpiSummaryJson = JsonSerializer.Serialize(summary),
            CreatedAt = DateTime.UtcNow
        };

        await _scorecardRepository.AddAsync(scorecard);
    }
}
