using System;
using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Modules.CustomerFeedback.Entities;

public enum SurveyQuestionType
{
    Text,
    Rating5,
    Rating10,
    MultipleChoice,
    Boolean
}

public class SurveyQuestion : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid SurveyId { get; set; }

    [Required]
    [MaxLength(500)]
    public string QuestionText { get; set; } = string.Empty;

    [Required]
    public SurveyQuestionType QuestionType { get; set; } = SurveyQuestionType.Rating5;

    public int OrderIndex { get; set; } = 1;

    public bool IsRequired { get; set; } = true;

    // JSON string for options if MultipleChoice e.g. ["Excellent", "Good", "Poor"]
    public string OptionsJson { get; set; } = "[]";
}
