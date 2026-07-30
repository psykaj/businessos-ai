using System;

namespace backend.Modules.ExecutiveInsights.DTOs;

public class ExecutiveInsightDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public decimal BusinessImpact { get; set; }
    public decimal ConfidenceLevel { get; set; }
    public string SuggestedAction { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public bool IsActioned { get; set; }
    public DateTime CreatedAt { get; set; }
}
