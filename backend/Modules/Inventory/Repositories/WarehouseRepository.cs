using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Inventory.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly ApplicationDbContext _context;

    public WarehouseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Warehouse?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Warehouses
            .Include(w => w.StockItems)
            .FirstOrDefaultAsync(w => w.Id == id && w.OrganizationId == organizationId && !w.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Warehouse>> GetAllAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Warehouses
            .Include(w => w.StockItems)
            .Where(w => w.OrganizationId == organizationId && !w.IsDeleted)
            .OrderByDescending(w => w.IsPrimary)
            .ThenBy(w => w.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Warehouse?> GetPrimaryAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Warehouses
            .FirstOrDefaultAsync(w => w.OrganizationId == organizationId && w.IsPrimary && !w.IsDeleted, cancellationToken);
    }

    public async Task AddAsync(Warehouse warehouse, CancellationToken cancellationToken = default)
    {
        await _context.Warehouses.AddAsync(warehouse, cancellationToken);
    }

    public void Update(Warehouse warehouse)
    {
        _context.Warehouses.Update(warehouse);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
