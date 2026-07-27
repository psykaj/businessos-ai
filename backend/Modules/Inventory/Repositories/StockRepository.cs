using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Inventory.Repositories;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _context;

    public StockRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryStock?> GetByProductAndWarehouseAsync(Guid productId, Guid warehouseId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryStocks
            .Include(s => s.Product)
            .Include(s => s.Warehouse)
            .FirstOrDefaultAsync(s => s.ProductId == productId && s.WarehouseId == warehouseId && s.OrganizationId == organizationId && !s.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<InventoryStock>> GetStockByProductAsync(Guid productId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryStocks
            .Include(s => s.Warehouse)
            .Where(s => s.ProductId == productId && s.OrganizationId == organizationId && !s.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InventoryStock>> GetStockByWarehouseAsync(Guid warehouseId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryStocks
            .Include(s => s.Product)
            .Where(s => s.WarehouseId == warehouseId && s.OrganizationId == organizationId && !s.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(InventoryStock stock, CancellationToken cancellationToken = default)
    {
        await _context.InventoryStocks.AddAsync(stock, cancellationToken);
    }

    public void Update(InventoryStock stock)
    {
        _context.InventoryStocks.Update(stock);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
