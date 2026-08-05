using Microsoft.EntityFrameworkCore;
using backend.Modules.Warehouses.Entities;
using backend.Modules.Warehouses.Interfaces;
using backend.Persistence;

namespace backend.Modules.Warehouses.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly ApplicationDbContext _context;

    public WarehouseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Warehouse?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.BranchWarehouses
            .Where(x => x.Id == id && x.OrganizationId == organizationId && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Warehouse>> GetAllByOrgAsync(Guid organizationId, Guid? branchId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.BranchWarehouses
            .Where(x => x.OrganizationId == organizationId && !x.IsDeleted);

        if (branchId.HasValue)
        {
            query = query.Where(x => x.BranchId == branchId.Value);
        }

        return await query.OrderBy(x => x.Name).ToListAsync(cancellationToken);
    }

    public async Task<Warehouse> AddAsync(Warehouse warehouse, CancellationToken cancellationToken = default)
    {
        await _context.BranchWarehouses.AddAsync(warehouse, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return warehouse;
    }

    public async Task UpdateAsync(Warehouse warehouse, CancellationToken cancellationToken = default)
    {
        _context.BranchWarehouses.Update(warehouse);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Warehouse warehouse, CancellationToken cancellationToken = default)
    {
        _context.BranchWarehouses.Remove(warehouse);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.BranchWarehouses
            .AnyAsync(x => x.Id == id && x.OrganizationId == organizationId && !x.IsDeleted, cancellationToken);
    }
}
