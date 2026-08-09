using System;
using backend.Common;

namespace backend.Modules.BusinessIntelligence.Entities;

public class ProactiveAlert : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string Type { get; set; } = string.Empty;
    
    public AlertSeverity Severity { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string SourceModule { get; set; } = string.Empty;
    
    public string SourceEntityId { get; set; } = string.Empty;
    
    public string RecommendedAction { get; set; } = string.Empty;
    
    public AlertStatus Status { get; set; }
    
    public DateTime? ReadAt { get; set; }
    
    public DateTime? ResolvedAt { get; set; }
    
    public string DeduplicationKey { get; set; } = string.Empty;
}
