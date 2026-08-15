using System;

namespace backend.Modules.BusinessGoals.DTOs;

public class BusinessKPIDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid BusinessGoalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string KPIType { get; set; } = string.Empty;
    public string MetricKey { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal CurrentValue { get; set; }
    public decimal TargetValue { get; set; }
    public decimal PreviousValue { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime LastCalculatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}
