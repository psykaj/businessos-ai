using System;

namespace backend.Modules.DecisionCenter.DTOs;

public class DecisionLogDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string DecisionTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? RelatedInsightId { get; set; }
    public Guid? DecisionMakerId { get; set; }
    public string ExpectedOutcome { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime DecisionDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
