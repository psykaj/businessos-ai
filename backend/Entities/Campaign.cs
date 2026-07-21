using backend.Common;

namespace backend.Entities;

public class Campaign : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }
    
    // Add additional base properties depending on the module
    public string Name { get; set; } = string.Empty;
    public string CampaignType { get; set; } = "Email"; 
    public string Source { get; set; } = string.Empty;
    public string Medium { get; set; } = string.Empty;
    public decimal Budget { get; set; } = 0;
    public string Status { get; set; } = "Draft"; // Draft, Scheduled, Running, Completed

    public Guid? TemplateId { get; set; }
    public DateTime? ScheduledAt { get; set; }

    public int TotalMessages { get; set; } = 0;
    public int SentMessages { get; set; } = 0;
    public int DeliveredMessages { get; set; } = 0;
    public int ReadMessages { get; set; } = 0;
    public int FailedMessages { get; set; } = 0;
}
