using backend.Common;
using backend.Entities;

namespace backend.Modules.EventBus.Entities;

public class EventSubscription : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public string EventType { get; set; } = string.Empty; // e.g. "customer.created", "invoice.paid"
    
    // Who is subscribing to this event? (Internal module, Webhook, Connector)
    public string SubscriberType { get; set; } = string.Empty; 
    public string SubscriberIdentifier { get; set; } = string.Empty; // WebhookEndpointId, ConnectorId, etc.
    
    public string Status { get; set; } = "Active"; // Active, Inactive
}
