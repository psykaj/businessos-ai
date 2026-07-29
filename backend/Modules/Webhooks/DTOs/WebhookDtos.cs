namespace backend.Modules.Webhooks.DTOs;

public class WebhookSubscriptionDto
{
    public Guid Id { get; set; }
    public string EventTypes { get; set; } = string.Empty;
    public string EndpointUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateWebhookSubscriptionDto
{
    public string EventTypes { get; set; } = string.Empty;
    public string EndpointUrl { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
}
