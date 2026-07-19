using backend.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Organization.Repositories;

public class OrganizationRepository : backend.Repositories.GenericRepository<Entities.Organization>, IOrganizationRepository
{
    private readonly ApplicationDbContext _context;

    public OrganizationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Entities.Organization?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Organizations
            .FirstOrDefaultAsync(o => o.Slug == slug && !o.IsDeleted, cancellationToken);
    }

    public async Task<(IReadOnlyList<Entities.Organization> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? searchTerm, CancellationToken cancellationToken = default)
    {
        var query = _context.Organizations
            .Where(o => !o.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(o => o.Name.Contains(searchTerm) || o.Slug.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
