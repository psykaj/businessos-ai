using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace backend.Modules.BusinessGoals.DTOs;

public class CreateBusinessGoalDto
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string GoalType { get; set; } = string.Empty;

    public decimal TargetValue { get; set; }
    
    [MaxLength(20)]
    public string Unit { get; set; } = string.Empty;

    [MaxLength(10)]
    public string Currency { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    
    public DateTime TargetDate { get; set; }

    [MaxLength(20)]
    public string Priority { get; set; } = "Medium";

    public Guid? OwnerUserId { get; set; }

    public string Metadata { get; set; } = "{}";

    public List<CreateBusinessKPIDto> KPIs { get; set; } = new List<CreateBusinessKPIDto>();
}

public class CreateBusinessKPIDto
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string KPIType { get; set; } = string.Empty;

    [MaxLength(100)]
    public string MetricKey { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal TargetValue { get; set; }

    [MaxLength(20)]
    public string Unit { get; set; } = string.Empty;

    [MaxLength(10)]
    public string Currency { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Direction { get; set; } = "HigherIsBetter";

    [MaxLength(20)]
    public string Frequency { get; set; } = "Monthly";
}
