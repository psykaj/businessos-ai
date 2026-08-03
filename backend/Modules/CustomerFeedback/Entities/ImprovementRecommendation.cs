using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.CustomerFeedback.Entities;

public enum RecommendationCategory
{
    ContactUnhappyCustomers,
    ImproveResponseTime,
    FollowUpAfterPurchase,
    ImproveProductQuality,
    RewardLoyalCustomers
}

public enum RecommendationPriority
{
    Low,
    Medium,
    High,
    Urgent
}

public enum RecommendationStatus
{
    Pending,
    InProgress,
    Completed,
    Dismissed
}

public class ImprovementRecommendation : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public RecommendationCategory Category { get; set; } = RecommendationCategory.ContactUnhappyCustomers;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? ActionPlan { get; set; }

    [Required]
    public RecommendationPriority Priority { get; set; } = RecommendationPriority.Medium;

    [Required]
    [MaxLength(200)]
    public string BusinessImpact { get; set; } = string.Empty; // e.g., "$15,000 ARR at risk of churn protected"

    public int EstimatedCustomerImpactCount { get; set; } = 0;

    [Required]
    public RecommendationStatus Status { get; set; } = RecommendationStatus.Pending;

    public Guid? TargetCustomerId { get; set; }

    [MaxLength(100)]
    public string? TargetEntityId { get; set; }
}
