using System;
using System.Threading.Tasks;
using backend.Modules.BusinessGoals.Repositories;
using backend.Modules.KpiEngine.Repositories;

namespace backend.Modules.BusinessGoals.Services;

public class GoalTrackingService
{
    private readonly IBusinessGoalRepository _goalRepository;
    private readonly IKpiRepository _kpiRepository;

    public GoalTrackingService(IBusinessGoalRepository goalRepository, IKpiRepository kpiRepository)
    {
        _goalRepository = goalRepository;
        _kpiRepository = kpiRepository;
    }

    public async Task UpdateGoalProgressAsync(Guid organizationId, Guid goalId)
    {
        var goal = await _goalRepository.GetByIdAsync(goalId, organizationId);
        if (goal == null || goal.KpiId == Guid.Empty) return;

        var kpi = await _kpiRepository.GetByIdAsync(goal.KpiId, organizationId);
        if (kpi == null) return;

        if (goal.TargetValue > 0)
        {
            goal.ProgressPercentage = (kpi.CurrentValue / goal.TargetValue) * 100;
            if (goal.ProgressPercentage > 100) goal.ProgressPercentage = 100;
            
            if (goal.ProgressPercentage >= 100)
                goal.Status = "Achieved";
            else if (goal.ProgressPercentage >= 75)
                goal.Status = "OnTrack";
            else if (goal.ProgressPercentage < 50 && DateTime.UtcNow > goal.StartDate.AddDays((goal.EndDate - goal.StartDate).TotalDays / 2))
                goal.Status = "AtRisk";
        }

        await _goalRepository.UpdateAsync(goal);
    }
}
