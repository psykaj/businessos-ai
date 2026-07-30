using System;

namespace backend.Modules.Scorecards.DTOs;

public class ScorecardDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public decimal OverallScore { get; set; }
    public string KpiSummaryJson { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
