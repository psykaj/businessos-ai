using Microsoft.EntityFrameworkCore;
using backend.Modules.Locations.Entities;
using backend.Modules.Locations.Interfaces;
using backend.Persistence;

namespace backend.Modules.Locations.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _context;

    public LocationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Location?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Locations
            .Where(x => x.Id == id && x.OrganizationId == organizationId && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Location>> GetAllByOrgAsync(Guid organizationId, string? region = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Locations
            .Where(x => x.OrganizationId == organizationId && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(region))
        {
            query = query.Where(x => x.Region.ToLower() == region.ToLower());
        }

        return await query.OrderBy(x => x.Name).ToListAsync(cancellationToken);
    }

    public async Task<Location> AddAsync(Location location, CancellationToken cancellationToken = default)
    {
        await _context.Locations.AddAsync(location, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return location;
    }

    public async Task UpdateAsync(Location location, CancellationToken cancellationToken = default)
    {
        _context.Locations.Update(location);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Location location, CancellationToken cancellationToken = default)
    {
        _context.Locations.Remove(location);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Locations
            .AnyAsync(x => x.Id == id && x.OrganizationId == organizationId && !x.IsDeleted, cancellationToken);
    }
}
