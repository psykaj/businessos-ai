using backend.Entities;
using backend.Interfaces;

namespace backend.Modules.Permissions.Repositories;

public interface IPermissionRepository : IGenericRepository<Permission>
{
    Task<IReadOnlyList<Permission>> GetByModuleAsync(string module, CancellationToken cancellationToken = default);
}
