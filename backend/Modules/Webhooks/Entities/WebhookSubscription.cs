using backend.Common;
using backend.Entities;

namespace backend.Modules.Webhooks.Entities;

public class WebhookSubscription : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public string EventType { get; set; } = string.Empty; // e.g. FormSubmitted, LeadCreated, CustomerConverted
    public string Url { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string Secret { get; set; } = string.Empty; // Used to sign the payload
}
