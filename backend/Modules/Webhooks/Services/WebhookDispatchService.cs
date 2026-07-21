using System.Text.Json;
using backend.Interfaces;
using backend.Modules.Webhooks.Entities;
using backend.Modules.Webhooks.Interfaces;

namespace backend.Modules.Webhooks.Services;

public class WebhookDispatchService : IWebhookDispatchService
{
    private readonly IWebhookRepository _webhookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly WebhookQueue _webhookQueue;

    public WebhookDispatchService(
        IWebhookRepository webhookRepository,
        IUnitOfWork unitOfWork,
        WebhookQueue webhookQueue)
    {
        _webhookRepository = webhookRepository;
        _unitOfWork = unitOfWork;
        _webhookQueue = webhookQueue;
    }

    public async Task EnqueueEventAsync(Guid organizationId, string eventType, object payload, CancellationToken cancellationToken = default)
    {
        var subscriptions = await _webhookRepository.GetActiveSubscriptionsByEventAsync(organizationId, eventType, cancellationToken);
        
        if (!subscriptions.Any())
            return;

        var jsonPayload = JsonSerializer.Serialize(payload);

        foreach (var sub in subscriptions)
        {
            var delivery = new WebhookDelivery
            {
                SubscriptionId = sub.Id,
                Payload = jsonPayload,
                AttemptCount = 0,
                Status = "Pending"
            };

            await _webhookRepository.AddDeliveryAsync(delivery, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Queue the delivery ID for the background worker
            await _webhookQueue.EnqueueAsync(delivery.Id, cancellationToken);
        }
    }
}
