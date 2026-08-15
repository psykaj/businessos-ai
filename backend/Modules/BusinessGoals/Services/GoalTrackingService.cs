using System;
using System.Threading.Tasks;
using backend.Modules.BusinessGoals.Repositories;

namespace backend.Modules.BusinessGoals.Services;

public class GoalTrackingService
{
    private readonly IBusinessGoalRepository _goalRepository;

    public GoalTrackingService(IBusinessGoalRepository goalRepository)
    {
        _goalRepository = goalRepository;
    }

    public async Task UpdateGoalProgressAsync(Guid organizationId, Guid goalId)
    {
        var goal = await _goalRepository.GetByIdAsync(goalId, organizationId);
        if (goal == null) return;

        if (goal.TargetValue > 0)
        {
            goal.ProgressPercentage = (goal.CurrentValue / goal.TargetValue) * 100;
            if (goal.ProgressPercentage > 100) goal.ProgressPercentage = 100;
            
            if (goal.ProgressPercentage >= 100)
                goal.Status = "Achieved";
            else if (goal.ProgressPercentage >= 75)
                goal.Status = "OnTrack";
            else if (goal.ProgressPercentage < 50 && DateTime.UtcNow > goal.StartDate.AddDays((goal.TargetDate - goal.StartDate).TotalDays / 2))
                goal.Status = "AtRisk";
        }

        await _goalRepository.UpdateAsync(goal);
    }
}
