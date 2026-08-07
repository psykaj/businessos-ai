using System;
using backend.Common;
using backend.Entities;

namespace backend.Modules.ActionCenter.Entities;

public class AiAction : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public AiActionCategory Category { get; set; }
    
    public AiActionPriority Priority { get; set; }
    
    public string BusinessImpact { get; set; } = string.Empty;
    
    public decimal EstimatedRevenueIncrease { get; set; }
    
    public decimal EstimatedCostSaving { get; set; }
    
    public AiActionRiskLevel RiskLevel { get; set; }
    
    public AiActionStatus Status { get; set; } = AiActionStatus.Pending;
    
    public DateTime? ExecutedDate { get; set; }
    
    public Guid? ApprovedBy { get; set; }
    
    public string? ExecutionResult { get; set; }
}
