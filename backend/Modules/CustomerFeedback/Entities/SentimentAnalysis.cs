using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.CustomerFeedback.Entities;

public enum SentimentClassification
{
    Positive,
    Neutral,
    Negative
}

public enum SentimentTargetType
{
    Feedback,
    SurveyResponse,
    Rating,
    Message
}

public class SentimentAnalysis : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public SentimentTargetType TargetEntityType { get; set; } = SentimentTargetType.Feedback;

    [Required]
    public Guid TargetEntityId { get; set; }

    [Required]
    public SentimentClassification Sentiment { get; set; } = SentimentClassification.Neutral;

    [Column(TypeName = "decimal(4, 3)")]
    public decimal ConfidenceScore { get; set; } = 0.0m; // between 0.000 and 1.000

    [Column(TypeName = "decimal(4, 2)")]
    public decimal UrgencyScore { get; set; } = 0.0m; // between 0.00 and 10.00

    [MaxLength(500)]
    public string? MainComplaint { get; set; }

    [MaxLength(500)]
    public string? SuggestedImprovement { get; set; }

    public string RawModelOutputJson { get; set; } = "{}";

    [MaxLength(50)]
    public string EngineProvider { get; set; } = "ProviderIndependentAI";
}
