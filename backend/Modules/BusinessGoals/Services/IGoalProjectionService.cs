using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessGoals.Entities;

namespace backend.Modules.BusinessGoals.Services;

public class ProjectionResult
{
    public decimal RequiredDailyRate { get; set; }
    public decimal RequiredWeeklyRate { get; set; }
    public decimal CurrentRate { get; set; }
    public decimal ProjectedValue { get; set; }
    public DateTime? ProjectedCompletionDate { get; set; }
    public string Status { get; set; } = string.Empty;
}

public interface IGoalProjectionService
{
    Task<ProjectionResult> ProjectGoalOutcomeAsync(BusinessGoal goal, CancellationToken cancellationToken = default);
}
