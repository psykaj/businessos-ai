using backend.Common;
using backend.Entities;

namespace backend.Modules.Webhooks.Entities;

public class WebhookDeliveryLog : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public Guid WebhookEndpointId { get; set; }
    public WebhookEndpoint? WebhookEndpoint { get; set; }

    public string EventType { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty; // JSON
    public string? ResponseHeaders { get; set; } // JSON
    public string? ResponseBody { get; set; }
    public int? StatusCode { get; set; }
    
    public int RetryCount { get; set; } = 0;
    public string Status { get; set; } = "Pending"; // Pending, Success, Failed
    public DateTime? DeliveredAt { get; set; }
}
