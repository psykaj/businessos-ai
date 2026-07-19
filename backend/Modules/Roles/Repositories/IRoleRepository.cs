using backend.Entities;
using backend.Interfaces;

namespace backend.Modules.Roles.Repositories;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Role>> GetSystemRolesAsync(CancellationToken cancellationToken = default);
}
