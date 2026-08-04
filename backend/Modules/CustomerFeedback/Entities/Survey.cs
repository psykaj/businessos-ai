using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.CustomerFeedback.Entities;

public enum SurveyType
{
    CSAT,       // Customer Satisfaction Score
    NPS,        // Net Promoter Score
    CES,        // Customer Effort Score
    Custom      // Custom survey form
}

public class Survey : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public SurveyType Type { get; set; } = SurveyType.CSAT;

    public bool IsActive { get; set; } = true;

    public DateTime ValidFrom { get; set; } = DateTime.UtcNow;

    public DateTime? ValidUntil { get; set; }

    public int TotalSent { get; set; } = 0;

    public int TotalCompleted { get; set; } = 0;

    [Column(TypeName = "decimal(5, 2)")]
    public decimal CompletionRate { get; set; } = 0m; // Calculated as (TotalCompleted / TotalSent) * 100

    [Column(TypeName = "decimal(5, 2)")]
    public decimal AverageScore { get; set; } = 0m;
}
