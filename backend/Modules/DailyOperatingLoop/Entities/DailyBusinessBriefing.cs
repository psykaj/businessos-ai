using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.DailyOperatingLoop.Entities;

public class DailyBusinessBriefing : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; } // Uses OrganizationId as BusinessId
    
    [Required]
    public DateTime BriefingDate { get; set; }
    
    public string Summary { get; set; } = string.Empty;
    
    public BusinessHealthState BusinessHealth { get; set; }
    
    public int PriorityCount { get; set; }
    
    public int OpportunityCount { get; set; }
    
    public int RiskCount { get; set; }
    
    public int CompletedPriorityCount { get; set; }
    
    public DateTime GeneratedAt { get; set; }
    
    public DateTime ExpiresAt { get; set; }
    
    [MaxLength(50)]
    public string Status { get; set; } = "Active"; // Active, Expired
    
    public ICollection<DailyPriority> Priorities { get; set; } = new List<DailyPriority>();
}
