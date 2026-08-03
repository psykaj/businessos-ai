using System;

namespace backend.Modules.CustomerFeedback.ImprovementActions.DTOs;

public class ImprovementRecommendationDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Category { get; set; } = "ContactUnhappyCustomers";
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ActionPlan { get; set; }
    public string Priority { get; set; } = "Medium";
    public string BusinessImpact { get; set; } = string.Empty;
    public int EstimatedCustomerImpactCount { get; set; }
    public string Status { get; set; } = "Pending";
    public Guid? TargetCustomerId { get; set; }
    public string? TargetEntityId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GenerateRecommendationsRequestDto
{
    public Guid OrganizationId { get; set; }
}
