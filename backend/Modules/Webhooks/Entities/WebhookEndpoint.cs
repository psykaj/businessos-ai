using backend.Common;
using backend.Entities;

namespace backend.Modules.Webhooks.Entities;

public class WebhookEndpoint : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public string Name { get; set; } = string.Empty;
    public string EndpointUrl { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty; // Used for payload signature
    
    // Comma-separated list of event types (e.g. "customer.created,invoice.paid")
    public string EventTypes { get; set; } = string.Empty;
    
    public string Status { get; set; } = "Active"; // Active, Inactive, Failing
    public string? Description { get; set; }
}
