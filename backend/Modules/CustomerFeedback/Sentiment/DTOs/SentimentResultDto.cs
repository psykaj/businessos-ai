using System;

namespace backend.Modules.CustomerFeedback.Sentiment.DTOs;

public class SentimentResultDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string TargetEntityType { get; set; } = string.Empty;
    public Guid TargetEntityId { get; set; }
    public string Sentiment { get; set; } = "Neutral";
    public decimal ConfidenceScore { get; set; }
    public decimal UrgencyScore { get; set; }
    public string? MainComplaint { get; set; }
    public string? SuggestedImprovement { get; set; }
    public string EngineProvider { get; set; } = "ProviderIndependentAI";
    public DateTime CreatedAt { get; set; }
}

public class SentimentAnalyzeRequestDto
{
    public Guid OrganizationId { get; set; }
    public Guid TargetEntityId { get; set; }
    public string TargetEntityType { get; set; } = "Feedback";
    public string TextToAnalyze { get; set; } = string.Empty;
}
