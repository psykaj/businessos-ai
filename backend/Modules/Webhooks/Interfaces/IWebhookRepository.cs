using backend.Interfaces;
using backend.Modules.Webhooks.Entities;

namespace backend.Modules.Webhooks.Interfaces;

public interface IWebhookRepository : IGenericRepository<WebhookSubscription>
{
    Task<IReadOnlyList<WebhookSubscription>> GetActiveSubscriptionsByEventAsync(Guid organizationId, string eventType, CancellationToken cancellationToken = default);
    
    Task<WebhookDelivery?> GetDeliveryByIdAsync(Guid deliveryId, CancellationToken cancellationToken = default);
    
    Task AddDeliveryAsync(WebhookDelivery delivery, CancellationToken cancellationToken = default);
    Task UpdateDeliveryAsync(WebhookDelivery delivery);
    
    Task<(IReadOnlyList<WebhookDelivery> Items, int TotalCount)> GetDeliveriesPagedAsync(Guid organizationId, Guid subscriptionId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
