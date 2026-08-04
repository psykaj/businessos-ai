using System;
using System.Collections.Generic;

namespace backend.Modules.CustomerFeedback.Feedback.DTOs;

public class FeedbackDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string Source { get; set; } = "WebWidget";
    public string Type { get; set; } = "General";
    public string Comment { get; set; } = string.Empty;
    public int? RatingValue { get; set; }
    public string Status { get; set; } = "New";
    public Guid? AssignedUserId { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolutionNotes { get; set; }
    public bool IsUrgent { get; set; }
    public string Tags { get; set; } = "[]";
    public bool HasBeenAnalyzed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Attached sentiment summary if available
    public string? SentimentClassification { get; set; }
    public decimal? UrgencyScore { get; set; }
    public string? SuggestedAction { get; set; }
}

public class SubmitFeedbackRequestDto
{
    public Guid OrganizationId { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string Source { get; set; } = "WebWidget";
    public string Type { get; set; } = "General";
    public string Comment { get; set; } = string.Empty;
    public int? RatingValue { get; set; }
}

public class FeedbackSearchFilterDto
{
    public Guid OrganizationId { get; set; }
    public string? Query { get; set; }
    public int? RatingValue { get; set; }
    public Guid? CustomerId { get; set; }
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class FeedbackPagedResultDto
{
    public IEnumerable<FeedbackDto> Items { get; set; } = new List<FeedbackDto>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
