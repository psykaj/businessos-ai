namespace backend.Modules.Webhooks.DTOs;

public class WebhookSubscriptionDto
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateWebhookSubscriptionDto
{
    public string EventType { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
}
