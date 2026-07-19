using backend.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Roles.Repositories;

public class RoleRepository : backend.Repositories.GenericRepository<Role>, IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == name && !r.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Role>> GetSystemRolesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Where(r => r.IsSystemRole && !r.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}
