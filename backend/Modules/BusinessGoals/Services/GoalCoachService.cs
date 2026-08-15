using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessGoals.DTOs;
using backend.Modules.BusinessIntelligence.Services;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BusinessGoals.Services;

public class GoalCoachService : IGoalCoachService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IAiRecommendationEngine _aiRecommendationEngine;
    private readonly IGoalGapAnalysisService _gapAnalysisService;

    public GoalCoachService(
        ApplicationDbContext dbContext,
        IAiRecommendationEngine aiRecommendationEngine,
        IGoalGapAnalysisService gapAnalysisService)
    {
        _dbContext = dbContext;
        _aiRecommendationEngine = aiRecommendationEngine;
        _gapAnalysisService = gapAnalysisService;
    }

    public async Task<AiCoachResponseDto> GetGoalCoachingAsync(Guid organizationId, Guid goalId, CancellationToken cancellationToken = default)
    {
        var goal = await _dbContext.BusinessGoals
            .Include(g => g.KPIs)
            .FirstOrDefaultAsync(g => g.Id == goalId && g.OrganizationId == organizationId, cancellationToken);

        if (goal == null)
            throw new Exception("Goal not found");

        var gap = await _gapAnalysisService.AnalyzeGapAsync(goal, cancellationToken);

        // Fetch overall business recommendations to blend with goal context
        var recommendations = await _aiRecommendationEngine.GenerateRecommendationsAsync(organizationId, cancellationToken);

        var response = new AiCoachResponseDto
        {
            Summary = $"Goal '{goal.Name}' is currently {gap.Risk}.",
            StatusExplanation = gap.PrimaryDrivers,
            KeyDrivers = goal.KPIs.Select(k => $"{k.Name}: {k.CurrentValue} {k.Unit} (Target: {k.TargetValue})").ToList(),
            RecommendedActions = recommendations.Take(3).ToList(),
            Confidence = 0.85m // Simplified for MVP
        };

        if (gap.Risk == "Behind" || gap.Risk == "AtRisk")
        {
            response.Risks.Add($"You are {gap.Gap:0.##} {goal.Unit} away from the target.");
            response.Opportunities.Add("Consider adjusting the target date or boosting current efforts.");
        }
        else if (gap.Risk == "OnTrack" || gap.Risk == "Achieved")
        {
            response.Opportunities.Add("You are on track. Maintain current performance.");
        }

        // Add Outcomes mapping from BusinessMemory / Outcomes
        var outcomes = await _dbContext.BusinessOutcomes
            .Where(o => o.BusinessId == organizationId && o.Status == backend.Modules.Outcomes.Entities.OutcomeStatus.Active)
            .OrderByDescending(o => o.OccurredAt)
            .Take(2)
            .Select(o => $"{o.OutcomeType}: {o.RevenueImpact} impact")
            .ToListAsync(cancellationToken);

        response.RelevantOutcomes = outcomes;

        return response;
    }
}
