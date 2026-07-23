namespace backend.Modules.BusinessIntelligence.DTOs;

public class InsightDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public string BusinessImpact { get; set; } = string.Empty;
    public string SuggestedAction { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class GenerateInsightsResponseDto
{
    public int GeneratedCount { get; set; }
    public List<InsightDto> Insights { get; set; } = new();
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}
