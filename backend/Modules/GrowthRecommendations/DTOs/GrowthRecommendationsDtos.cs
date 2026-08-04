using System;
using System.Collections.Generic;

namespace backend.Modules.GrowthRecommendations.DTOs;

public class GrowthRecommendationDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string RecommendationType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CurrentState { get; set; } = string.Empty;
    public string SuggestedAction { get; set; } = string.Empty;
    public decimal EstimatedFinancialImpact { get; set; }
    public decimal ConfidenceScore { get; set; }
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ActionedAt { get; set; }
}

public class CreateGrowthRecommendationRequest
{
    public string RecommendationType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CurrentState { get; set; } = string.Empty;
    public string SuggestedAction { get; set; } = string.Empty;
    public decimal EstimatedFinancialImpact { get; set; }
    public decimal ConfidenceScore { get; set; }
    public string Priority { get; set; } = "High";
    public string Category { get; set; } = "Revenue";
}

public class UpdateRecommendationStatusRequest
{
    public string Status { get; set; } = "Actioned"; // Actioned, In-Progress, Dismissed
    public string? ActionNotes { get; set; }
}

public class GrowthRecommendationSummaryDto
{
    public Guid OrganizationId { get; set; }
    public decimal TotalPotentialRevenueImpact { get; set; }
    public int HighPriorityCount { get; set; }
    public int TotalPendingRecommendations { get; set; }
    public List<GrowthRecommendationDto> TopRecommendations { get; set; } = new();
}
