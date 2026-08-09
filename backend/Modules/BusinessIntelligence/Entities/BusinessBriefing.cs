using System;
using System.Collections.Generic;
using backend.Common;

namespace backend.Modules.BusinessIntelligence.Entities;

public class BusinessBriefing : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public DateTime Date { get; set; }
    
    public DateTime PeriodStart { get; set; }
    
    public DateTime PeriodEnd { get; set; }
    
    public string Summary { get; set; } = string.Empty;
    
    public int BusinessHealthScore { get; set; }
    
    public string RevenueSummary { get; set; } = string.Empty;
    
    public string CustomerSummary { get; set; } = string.Empty;
    
    public string FinanceSummary { get; set; } = string.Empty;
    
    public string InventorySummary { get; set; } = string.Empty;
    
    public int RiskCount { get; set; }
    
    public int OpportunityCount { get; set; }
    
    public int ActionCount { get; set; }
    
    public DateTime GeneratedAt { get; set; }
    
    public BriefingStatus Status { get; set; }
    
    public ICollection<BriefingItem> Items { get; set; } = new List<BriefingItem>();
}
