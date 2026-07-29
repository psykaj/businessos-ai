using backend.Modules.Webhooks.Entities;
using backend.Modules.Webhooks.Interfaces;
using backend.Persistence;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Webhooks.Repositories;

public class WebhookRepository : GenericRepository<WebhookEndpoint>, IWebhookRepository
{
    private readonly ApplicationDbContext _context;

    public WebhookRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<WebhookEndpoint>> GetActiveSubscriptionsByEventAsync(Guid organizationId, string eventType, CancellationToken cancellationToken = default)
    {
        return await _context.WebhookEndpoints
            .Where(ws => ws.OrganizationId == organizationId && ws.Status == "Active" && ws.EventTypes.Contains(eventType) && !ws.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<WebhookDeliveryLog?> GetDeliveryByIdAsync(Guid deliveryId, CancellationToken cancellationToken = default)
    {
        return await _context.WebhookDeliveryLogs.FirstOrDefaultAsync(d => d.Id == deliveryId && !d.IsDeleted, cancellationToken);
    }

    public async Task AddDeliveryAsync(WebhookDeliveryLog delivery, CancellationToken cancellationToken = default)
    {
        await _context.WebhookDeliveryLogs.AddAsync(delivery, cancellationToken);
    }

    public void UpdateDeliveryAsync(WebhookDeliveryLog delivery)
    {
        _context.WebhookDeliveryLogs.Update(delivery);
    }

    // Explicitly implementing the missing interface method signature
    Task IWebhookRepository.UpdateDeliveryAsync(WebhookDeliveryLog delivery)
    {
        _context.WebhookDeliveryLogs.Update(delivery);
        return Task.CompletedTask;
    }

    public async Task<(IReadOnlyList<WebhookDeliveryLog> Items, int TotalCount)> GetDeliveriesPagedAsync(Guid organizationId, Guid endpointId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.WebhookDeliveryLogs
            .Include(d => d.WebhookEndpoint)
            .Where(d => d.WebhookEndpointId == endpointId && d.WebhookEndpoint!.OrganizationId == organizationId && !d.IsDeleted);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
