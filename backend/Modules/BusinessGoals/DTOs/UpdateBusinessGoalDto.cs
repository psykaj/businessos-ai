using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Modules.BusinessGoals.DTOs;

public class UpdateBusinessGoalDto
{
    [MaxLength(150)]
    public string? Name { get; set; }

    public string? Description { get; set; }

    [MaxLength(50)]
    public string? GoalType { get; set; }

    public decimal? TargetValue { get; set; }
    
    [MaxLength(20)]
    public string? Unit { get; set; }

    [MaxLength(10)]
    public string? Currency { get; set; }

    public DateTime? TargetDate { get; set; }

    [MaxLength(20)]
    public string? Priority { get; set; }

    public Guid? OwnerUserId { get; set; }

    public string? Metadata { get; set; }
}
