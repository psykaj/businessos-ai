using System;
using System.Collections.Generic;

namespace backend.Modules.BusinessIntelligence.DTOs;

public class BusinessBriefingDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTime Date { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public string Summary { get; set; } = string.Empty;
    public int BusinessHealthScore { get; set; }
    public string RevenueSummary { get; set; } = string.Empty;
    public string CustomerSummary { get; set; } = string.Empty;
    public string FinanceSummary { get; set; } = string.Empty;
    public string InventorySummary { get; set; } = string.Empty;
    public int RiskCount { get; set; }
    public int OpportunityCount { get; set; }
    public int ActionCount { get; set; }
    public DateTime GeneratedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    
    public List<BriefingItemDto> Items { get; set; } = new();
}

public class BriefingItemDto
{
    public Guid Id { get; set; }
    public Guid BriefingId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Priority { get; set; }
    public string Category { get; set; } = string.Empty;
    public string SourceModule { get; set; } = string.Empty;
    public string SourceEntityId { get; set; } = string.Empty;
    public decimal ConfidenceScore { get; set; }
    public string ExpectedBusinessImpact { get; set; } = string.Empty;
    public string RecommendedAction { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public bool IsDismissed { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ProactiveAlertDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SourceModule { get; set; } = string.Empty;
    public string SourceEntityId { get; set; } = string.Empty;
    public string RecommendedAction { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? ReadAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GenerateBriefingRequest
{
    public DateTime? Date { get; set; }
    public bool ForceRegeneration { get; set; }
}

public class BriefingActionRequest
{
    public string Action { get; set; } = string.Empty;
}
