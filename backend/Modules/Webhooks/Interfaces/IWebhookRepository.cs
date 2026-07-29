using backend.Interfaces;
using backend.Modules.Webhooks.Entities;

namespace backend.Modules.Webhooks.Interfaces;

public interface IWebhookRepository : IGenericRepository<WebhookEndpoint>
{
    Task<IReadOnlyList<WebhookEndpoint>> GetActiveSubscriptionsByEventAsync(Guid organizationId, string eventType, CancellationToken cancellationToken = default);
    
    Task<WebhookDeliveryLog?> GetDeliveryByIdAsync(Guid deliveryId, CancellationToken cancellationToken = default);
    
    Task AddDeliveryAsync(WebhookDeliveryLog delivery, CancellationToken cancellationToken = default);
    Task UpdateDeliveryAsync(WebhookDeliveryLog delivery);
    
    Task<(IReadOnlyList<WebhookDeliveryLog> Items, int TotalCount)> GetDeliveriesPagedAsync(Guid organizationId, Guid endpointId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
