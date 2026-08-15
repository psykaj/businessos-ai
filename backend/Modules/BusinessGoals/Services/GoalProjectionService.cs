using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessGoals.Entities;

namespace backend.Modules.BusinessGoals.Services;

public class GoalProjectionService : IGoalProjectionService
{
    public Task<ProjectionResult> ProjectGoalOutcomeAsync(BusinessGoal goal, CancellationToken cancellationToken = default)
    {
        var result = new ProjectionResult();
        
        if (goal == null)
            return Task.FromResult(result);

        var now = DateTime.UtcNow;

        if (goal.StartDate >= goal.TargetDate)
        {
            result.Status = "InvalidDates";
            return Task.FromResult(result);
        }

        var totalDays = (goal.TargetDate - goal.StartDate).TotalDays;
        var elapsedDays = (now - goal.StartDate).TotalDays;
        var remainingDays = (goal.TargetDate - now).TotalDays;

        if (elapsedDays <= 0)
        {
            result.Status = "NotStarted";
            return Task.FromResult(result);
        }

        var remainingValue = goal.TargetValue - goal.CurrentValue;

        if (remainingDays <= 0)
        {
            result.Status = remainingValue <= 0 ? "Achieved" : "Behind";
            result.RequiredDailyRate = 0;
            result.RequiredWeeklyRate = 0;
            result.CurrentRate = goal.CurrentValue / (decimal)elapsedDays;
            result.ProjectedValue = goal.CurrentValue;
            return Task.FromResult(result);
        }

        result.CurrentRate = goal.CurrentValue / (decimal)elapsedDays;

        if (remainingValue <= 0)
        {
            result.Status = "Achieved";
            result.RequiredDailyRate = 0;
            result.RequiredWeeklyRate = 0;
            result.ProjectedValue = goal.CurrentValue + (result.CurrentRate * (decimal)remainingDays);
            return Task.FromResult(result);
        }

        result.RequiredDailyRate = remainingValue / (decimal)remainingDays;
        result.RequiredWeeklyRate = result.RequiredDailyRate * 7;
        
        result.ProjectedValue = goal.CurrentValue + (result.CurrentRate * (decimal)remainingDays);

        if (result.CurrentRate > 0)
        {
            var daysToCompletion = (double)(remainingValue / result.CurrentRate);
            result.ProjectedCompletionDate = now.AddDays(daysToCompletion);
        }

        if (result.ProjectedValue >= goal.TargetValue)
        {
            result.Status = "OnTrack";
        }
        else if (result.ProjectedValue >= goal.TargetValue * 0.9m)
        {
            result.Status = "AtRisk";
        }
        else
        {
            result.Status = "Behind";
        }

        return Task.FromResult(result);
    }
}
