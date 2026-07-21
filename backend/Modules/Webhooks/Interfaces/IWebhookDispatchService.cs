namespace backend.Modules.Webhooks.Interfaces;

public interface IWebhookDispatchService
{
    Task EnqueueEventAsync(Guid organizationId, string eventType, object payload, CancellationToken cancellationToken = default);
}
