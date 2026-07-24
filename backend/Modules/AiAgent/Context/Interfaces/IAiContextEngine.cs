using backend.Modules.AiAgent.Context.Models;

namespace backend.Modules.AiAgent.Context.Interfaces;

public interface IAiContextEngine
{
    Task<AiContext> BuildContextAsync(Guid organizationId, string userId, CancellationToken cancellationToken = default);
}
