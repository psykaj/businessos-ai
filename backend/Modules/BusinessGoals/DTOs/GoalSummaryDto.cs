using System;

namespace backend.Modules.BusinessGoals.DTOs;

public class GoalSummaryDto
{
    public int TotalGoals { get; set; }
    public int OnTrack { get; set; }
    public int AtRisk { get; set; }
    public int Behind { get; set; }
    public int Completed { get; set; }
    public BusinessGoalDto? TopPriorityGoal { get; set; }
    public decimal TotalProgress { get; set; }
}
