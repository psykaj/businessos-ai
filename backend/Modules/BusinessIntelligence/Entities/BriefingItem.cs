using System;
using backend.Common;

namespace backend.Modules.BusinessIntelligence.Entities;

public class BriefingItem : BaseEntity
{
    public Guid BriefingId { get; set; }
    
    public BriefingItemType Type { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public int Priority { get; set; }
    
    public string Category { get; set; } = string.Empty;
    
    public string SourceModule { get; set; } = string.Empty;
    
    public string SourceEntityId { get; set; } = string.Empty;
    
    public decimal ConfidenceScore { get; set; }
    
    public string ExpectedBusinessImpact { get; set; } = string.Empty;
    
    public string RecommendedAction { get; set; } = string.Empty;
    
    public bool IsRead { get; set; }
    
    public bool IsDismissed { get; set; }
    
    public BusinessBriefing Briefing { get; set; } = null!;
}
