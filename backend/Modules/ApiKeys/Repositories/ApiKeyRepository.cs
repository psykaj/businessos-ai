using backend.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.ApiKeys.Repositories;

public class ApiKeyRepository : backend.Repositories.GenericRepository<ApiKey>, IApiKeyRepository
{
    private readonly ApplicationDbContext _context;

    public ApiKeyRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ApiKey?> GetByKeyHashAsync(string keyHash, CancellationToken cancellationToken = default)
    {
        return await _context.ApiKeys
            .FirstOrDefaultAsync(k => k.KeyHash == keyHash && !k.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<ApiKey>> GetByOrganizationAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.ApiKeys
            .Where(k => k.OrganizationId == organizationId && !k.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}
