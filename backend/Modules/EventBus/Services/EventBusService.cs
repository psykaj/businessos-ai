using System.Text.Json;
using backend.Interfaces;
using backend.Modules.EventBus.Entities;
using backend.Modules.EventBus.Interfaces;
using backend.Modules.Webhooks.Interfaces;
using backend.Persistence;

namespace backend.Modules.EventBus.Services;

public class EventBusService : IEventBusService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IWebhookDispatchService _webhookDispatchService;
    private readonly IUnitOfWork _unitOfWork;

    public EventBusService(
        ApplicationDbContext dbContext,
        IWebhookDispatchService webhookDispatchService,
        IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _webhookDispatchService = webhookDispatchService;
        _unitOfWork = unitOfWork;
    }

    public async Task PublishAsync<T>(string eventType, Guid organizationId, T payload, string source, CancellationToken cancellationToken = default)
    {
        var jsonPayload = JsonSerializer.Serialize(payload);

        // 1. Log the event
        var eventLog = new EventLog
        {
            OrganizationId = organizationId,
            EventType = eventType,
            Payload = jsonPayload,
            Source = source
        };
        _dbContext.EventLogs.Add(eventLog);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // 2. Dispatch to webhooks
        await _webhookDispatchService.EnqueueEventAsync(organizationId, eventType, payload, cancellationToken);
        
        // 3. (Future) Dispatch to Connectors/Integrations (e.g. Slack, Stripe)
    }
}
