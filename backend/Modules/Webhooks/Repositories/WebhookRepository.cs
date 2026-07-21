using backend.Modules.Webhooks.Entities;
using backend.Modules.Webhooks.Interfaces;
using backend.Persistence;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Webhooks.Repositories;

public class WebhookRepository : GenericRepository<WebhookSubscription>, IWebhookRepository
{
    private readonly ApplicationDbContext _context;

    public WebhookRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<WebhookSubscription>> GetActiveSubscriptionsByEventAsync(Guid organizationId, string eventType, CancellationToken cancellationToken = default)
    {
        return await _context.WebhookSubscriptions
            .Where(ws => ws.OrganizationId == organizationId && ws.IsActive && ws.EventType == eventType && !ws.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<WebhookDelivery?> GetDeliveryByIdAsync(Guid deliveryId, CancellationToken cancellationToken = default)
    {
        return await _context.WebhookDeliveries.FirstOrDefaultAsync(d => d.Id == deliveryId && !d.IsDeleted, cancellationToken);
    }

    public async Task AddDeliveryAsync(WebhookDelivery delivery, CancellationToken cancellationToken = default)
    {
        await _context.WebhookDeliveries.AddAsync(delivery, cancellationToken);
    }

    public void UpdateDeliveryAsync(WebhookDelivery delivery)
    {
        _context.WebhookDeliveries.Update(delivery);
    }

    // Explicitly implementing the missing interface method signature
    Task IWebhookRepository.UpdateDeliveryAsync(WebhookDelivery delivery)
    {
        _context.WebhookDeliveries.Update(delivery);
        return Task.CompletedTask;
    }

    public async Task<(IReadOnlyList<WebhookDelivery> Items, int TotalCount)> GetDeliveriesPagedAsync(Guid organizationId, Guid subscriptionId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.WebhookDeliveries
            .Include(d => d.Subscription)
            .Where(d => d.SubscriptionId == subscriptionId && d.Subscription!.OrganizationId == organizationId && !d.IsDeleted);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
