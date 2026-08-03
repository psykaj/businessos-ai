using System;

namespace backend.Modules.CustomerFeedback.Ratings.DTOs;

public class RatingDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string EntityType { get; set; } = "Service";
    public string EntityId { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public string? CustomerEmail { get; set; }
    public decimal RatingScore { get; set; }
    public int MaxScore { get; set; }
    public string? ReviewText { get; set; }
    public bool VerifiedPurchase { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SubmitRatingRequestDto
{
    public Guid OrganizationId { get; set; }
    public string EntityType { get; set; } = "Service";
    public string EntityId { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public string? CustomerEmail { get; set; }
    public decimal RatingScore { get; set; }
    public int MaxScore { get; set; } = 5;
    public string? ReviewText { get; set; }
    public bool VerifiedPurchase { get; set; } = false;
}

public class RatingSummaryDto
{
    public string EntityType { get; set; } = "Service";
    public string EntityId { get; set; } = string.Empty;
    public decimal AverageRating { get; set; }
    public int TotalRatings { get; set; }
    public int FiveStarCount { get; set; }
    public int FourStarCount { get; set; }
    public int ThreeStarCount { get; set; }
    public int TwoStarCount { get; set; }
    public int OneStarCount { get; set; }
}
