namespace backend.Modules.BusinessIntelligence.DTOs;

public class GoalDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal TargetValue { get; set; }
    public decimal CurrentValue { get; set; }
    public double ProgressPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateGoalDto
{
    public string Name { get; set; } = string.Empty;
    public decimal TargetValue { get; set; }
    public decimal InitialValue { get; set; } = 0;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class UpdateGoalDto
{
    public string? Name { get; set; }
    public decimal? TargetValue { get; set; }
    public decimal? CurrentValue { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; }
}
