using System;

namespace backend.Modules.BusinessGoals.DTOs;

public class GoalAnalysisDto
{
    public decimal Target { get; set; }
    public decimal Current { get; set; }
    public decimal Gap { get; set; }
    public decimal RequiredRate { get; set; }
    public decimal CurrentRate { get; set; }
    public decimal ProjectedValue { get; set; }
    public string Risk { get; set; } = string.Empty;
    public string PrimaryDrivers { get; set; } = string.Empty;
}
