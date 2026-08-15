using System;
using System.Collections.Generic;

namespace backend.Modules.BusinessGoals.DTOs;

public class BusinessGoalDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string GoalType { get; set; } = string.Empty;
    public decimal TargetValue { get; set; }
    public decimal CurrentValue { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime TargetDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public Guid? OwnerUserId { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsActive { get; set; }
    public string Metadata { get; set; } = string.Empty;
    public decimal ProgressPercentage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<BusinessKPIDto> KPIs { get; set; } = new List<BusinessKPIDto>();
}
