using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.CustomerFeedback.Entities;

public class SurveyResponse : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid SurveyId { get; set; }

    public Guid? CustomerId { get; set; }

    [MaxLength(150)]
    public string? CustomerEmail { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Score { get; set; } // Aggregate or key rating score (e.g. NPS score 0-10 or CSAT score 1-5)

    [Required]
    public string AnswersJson { get; set; } = "[]"; // List of { QuestionId, QuestionText, Answer }

    [MaxLength(50)]
    public string? SentimentStatus { get; set; } // Positive, Neutral, Negative
}
