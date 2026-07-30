using System;

namespace backend.Modules.AiRecommendations.DTOs;

public class AiRecommendationDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal EstimatedImpact { get; set; }
    public decimal ConfidenceLevel { get; set; }
    public string SuggestedAction { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public bool IsApplied { get; set; }
    public DateTime? AppliedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
