using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.Entities;

namespace backend.Modules.Workflow.Interfaces;

public interface IIntegrationRepository
{
    Task<Integration?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<Integration?> GetByProviderAsync(Guid organizationId, IntegrationProvider provider, CancellationToken cancellationToken = default);
    Task<List<Integration>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<Integration> AddAsync(Integration integration, CancellationToken cancellationToken = default);
    Task UpdateAsync(Integration integration, CancellationToken cancellationToken = default);
    Task DeleteAsync(Integration integration, CancellationToken cancellationToken = default);
}
