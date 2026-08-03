using System;
using System.Collections.Generic;

namespace backend.Modules.CustomerFeedback.Surveys.DTOs;

public class SurveyQuestionDto
{
    public Guid Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = "Rating5";
    public int OrderIndex { get; set; }
    public bool IsRequired { get; set; }
    public string OptionsJson { get; set; } = "[]";
}

public class SurveyDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Type { get; set; } = "CSAT";
    public bool IsActive { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }
    public int TotalSent { get; set; }
    public int TotalCompleted { get; set; }
    public decimal CompletionRate { get; set; }
    public decimal AverageScore { get; set; }
    public List<SurveyQuestionDto> Questions { get; set; } = new();
}

public class CreateSurveyQuestionRequestDto
{
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = "Rating5";
    public int OrderIndex { get; set; } = 1;
    public bool IsRequired { get; set; } = true;
    public string OptionsJson { get; set; } = "[]";
}

public class CreateSurveyRequestDto
{
    public Guid OrganizationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Type { get; set; } = "CSAT";
    public DateTime? ValidUntil { get; set; }
    public List<CreateSurveyQuestionRequestDto> Questions { get; set; } = new();
}

public class SurveyResponseDto
{
    public Guid Id { get; set; }
    public Guid SurveyId { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerEmail { get; set; }
    public DateTime SubmittedAt { get; set; }
    public decimal? Score { get; set; }
    public string AnswersJson { get; set; } = "[]";
    public string? SentimentStatus { get; set; }
}

public class SubmitSurveyResponseDto
{
    public Guid OrganizationId { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerEmail { get; set; }
    public decimal? Score { get; set; }
    public string AnswersJson { get; set; } = "[]";
}
