using backend.Common;

namespace backend.Modules.Webhooks.Entities;

public class WebhookDelivery : BaseEntity
{
    public Guid SubscriptionId { get; set; }
    public WebhookSubscription? Subscription { get; set; }

    public string Payload { get; set; } = string.Empty;
    public int AttemptCount { get; set; } = 0;
    public string Status { get; set; } = "Pending"; // Pending, Success, Failed
    public DateTime? LastAttemptAt { get; set; }
    public string LastError { get; set; } = string.Empty;
}
