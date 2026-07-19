using backend.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Permissions.Repositories;

public class PermissionRepository : backend.Repositories.GenericRepository<Permission>, IPermissionRepository
{
    private readonly ApplicationDbContext _context;

    public PermissionRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Permission>> GetByModuleAsync(string module, CancellationToken cancellationToken = default)
    {
        return await _context.Permissions
            .Where(p => p.Module == module && !p.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}
