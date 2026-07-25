using backend.Common;
using backend.Entities;

namespace backend.Modules.CustomerSuccess.Satisfaction.Entities;

public class CustomerFeedback : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }


    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public int Rating { get; set; } // 1 - 5 for CSAT, or 0 - 10 for NPS
    public string? Feedback { get; set; }
    public string Channel { get; set; } = "Web"; // Email, InApp, SMS, Web
    public string FeedbackType { get; set; } = "CSAT"; // CSAT, NPS

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}
