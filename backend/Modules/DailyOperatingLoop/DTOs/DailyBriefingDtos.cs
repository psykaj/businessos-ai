using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace backend.Modules.DailyOperatingLoop.DTOs;

public class DailyBusinessBriefingDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTime BriefingDate { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string BusinessHealth { get; set; } = string.Empty;
    public int PriorityCount { get; set; }
    public int OpportunityCount { get; set; }
    public int RiskCount { get; set; }
    public int CompletedPriorityCount { get; set; }
    public DateTime GeneratedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<DailyPriorityDto> Priorities { get; set; } = new();
}

public class DailyPriorityDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid BriefingId { get; set; }
    public string PriorityType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public decimal PriorityScore { get; set; }
    public string RelatedEntityType { get; set; } = string.Empty;
    public string RelatedEntityId { get; set; } = string.Empty;
    public Guid? RelatedGoalId { get; set; }
    public Guid? RelatedKpiId { get; set; }
    public string SuggestedAction { get; set; } = string.Empty;
    public string ExpectedImpact { get; set; } = string.Empty;
    public string ImpactType { get; set; } = string.Empty;
    public decimal Confidence { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? DueAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
