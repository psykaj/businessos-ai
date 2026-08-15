using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessGoals.DTOs;
using backend.Modules.BusinessGoals.Entities;

namespace backend.Modules.BusinessGoals.Services;

public interface IGoalGapAnalysisService
{
    Task<GoalAnalysisDto> AnalyzeGapAsync(BusinessGoal goal, CancellationToken cancellationToken = default);
}
