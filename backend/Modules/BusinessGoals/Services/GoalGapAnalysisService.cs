using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessGoals.DTOs;
using backend.Modules.BusinessGoals.Entities;

namespace backend.Modules.BusinessGoals.Services;

public class GoalGapAnalysisService : IGoalGapAnalysisService
{
    private readonly IGoalProjectionService _projectionService;

    public GoalGapAnalysisService(IGoalProjectionService projectionService)
    {
        _projectionService = projectionService;
    }

    public async Task<GoalAnalysisDto> AnalyzeGapAsync(BusinessGoal goal, CancellationToken cancellationToken = default)
    {
        var dto = new GoalAnalysisDto();

        if (goal == null)
            return dto;

        dto.Target = goal.TargetValue;
        dto.Current = goal.CurrentValue;
        dto.Gap = goal.TargetValue - goal.CurrentValue;

        var projection = await _projectionService.ProjectGoalOutcomeAsync(goal, cancellationToken);

        dto.CurrentRate = projection.CurrentRate;
        dto.RequiredRate = projection.RequiredDailyRate;
        dto.ProjectedValue = projection.ProjectedValue;
        dto.Risk = projection.Status;

        if (dto.Gap <= 0)
        {
            dto.Gap = 0;
            dto.Risk = "OnTrack";
            dto.PrimaryDrivers = "Target has been achieved.";
        }
        else
        {
            if (dto.Risk == "Behind")
            {
                dto.PrimaryDrivers = $"Current rate of {dto.CurrentRate:0.##} is below the required daily rate of {dto.RequiredRate:0.##}.";
            }
            else if (dto.Risk == "AtRisk")
            {
                dto.PrimaryDrivers = "Projection is close to target but slightly off pace. Small improvements needed.";
            }
            else
            {
                dto.PrimaryDrivers = "On pace to hit target.";
            }
        }

        return dto;
    }
}
