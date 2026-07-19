using backend.Entities;
using backend.Interfaces;

namespace backend.Modules.ApiKeys.Repositories;

public interface IApiKeyRepository : IGenericRepository<ApiKey>
{
    Task<ApiKey?> GetByKeyHashAsync(string keyHash, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ApiKey>> GetByOrganizationAsync(Guid organizationId, CancellationToken cancellationToken = default);
}
