using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.CustomerFeedback.Entities;

public enum FeedbackStatus
{
    New,
    InReview,
    Assigned,
    InProgress,
    Resolved,
    Dismissed
}

public enum FeedbackSource
{
    WebWidget,
    Email,
    InApp,
    Survey,
    SupportTicket,
    PublicForm,
    Api
}

public class Feedback : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    public Guid? CustomerId { get; set; }

    [MaxLength(150)]
    public string? CustomerName { get; set; }

    [MaxLength(150)]
    public string? CustomerEmail { get; set; }

    [Required]
    public FeedbackSource Source { get; set; } = FeedbackSource.WebWidget;

    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = "General"; // e.g., Complaint, Praise, FeatureRequest, BugReport

    [Required]
    public string Comment { get; set; } = string.Empty;

    public int? RatingValue { get; set; } // e.g. 1 to 5 or 1 to 10

    [Required]
    public FeedbackStatus Status { get; set; } = FeedbackStatus.New;

    public Guid? AssignedUserId { get; set; }

    public DateTime? ResolvedAt { get; set; }

    [MaxLength(2000)]
    public string? ResolutionNotes { get; set; }

    public bool IsUrgent { get; set; } = false;

    // Optional tags or categories stored as JSON or comma separated
    [MaxLength(500)]
    public string Tags { get; set; } = "[]";

    public bool HasBeenAnalyzed { get; set; } = false;
}
