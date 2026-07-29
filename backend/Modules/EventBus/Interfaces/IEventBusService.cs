using backend.Modules.EventBus.Entities;

namespace backend.Modules.EventBus.Interfaces;

public interface IEventBusService
{
    Task PublishAsync<T>(string eventType, Guid organizationId, T payload, string source, CancellationToken cancellationToken = default);
}
