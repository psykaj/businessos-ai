using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.DailyOperatingLoop.Entities;

public class DailyPriority : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; } // Uses OrganizationId as BusinessId
    
    [Required]
    public Guid BriefingId { get; set; }
    
    public DailyBusinessBriefing Briefing { get; set; } = null!;
    
    public PriorityType PriorityType { get; set; }
    
    [MaxLength(250)]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string Reason { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string Severity { get; set; } = "Medium"; // Low, Medium, High, Critical
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal PriorityScore { get; set; }
    
    [MaxLength(100)]
    public string RelatedEntityType { get; set; } = string.Empty; // e.g., "Invoice", "Customer", "Goal"
    
    [MaxLength(100)]
    public string RelatedEntityId { get; set; } = string.Empty;
    
    public Guid? RelatedGoalId { get; set; }
    
    public Guid? RelatedKpiId { get; set; }
    
    public string SuggestedAction { get; set; } = string.Empty;
    
    public string ExpectedImpact { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string ImpactType { get; set; } = string.Empty; // e.g., "Revenue", "Risk Mitigation", "Retention"
    
    [Column(TypeName = "decimal(5,2)")]
    public decimal Confidence { get; set; } // Percentage 0-100
    
    public PriorityStatus Status { get; set; }
    
    public DateTime? DueAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }
}
