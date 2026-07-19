using backend.Common;

namespace backend.Entities;

public class Campaign : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }
    
    // Add additional base properties depending on the module
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "WhatsApp"; // e.g. WhatsApp, Email
    public string Status { get; set; } = "Draft"; // Draft, Scheduled, Running, Completed

    public Guid? TemplateId { get; set; }
    public DateTime? ScheduledAt { get; set; }

    public int TotalMessages { get; set; } = 0;
    public int SentMessages { get; set; } = 0;
    public int DeliveredMessages { get; set; } = 0;
    public int ReadMessages { get; set; } = 0;
    public int FailedMessages { get; set; } = 0;
}
