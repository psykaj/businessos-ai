using backend.Common;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Inventory.Repositories;

public class StockAdjustmentRepository : IStockAdjustmentRepository
{
    private readonly ApplicationDbContext _context;

    public StockAdjustmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StockAdjustment?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.StockAdjustments
            .Include(sa => sa.Warehouse)
            .Include(sa => sa.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(sa => sa.Id == id && sa.OrganizationId == organizationId && !sa.IsDeleted, cancellationToken);
    }

    public async Task<PagedResult<StockAdjustment>> GetPagedAsync(Guid organizationId, Guid? warehouseId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.StockAdjustments
            .Include(sa => sa.Warehouse)
            .Include(sa => sa.Items)
                .ThenInclude(i => i.Product)
            .Where(sa => sa.OrganizationId == organizationId && !sa.IsDeleted);

        if (warehouseId.HasValue)
        {
            query = query.Where(sa => sa.WarehouseId == warehouseId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(sa => sa.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<StockAdjustment>(items, totalCount, pageNumber, pageSize);
    }

    public async Task AddAsync(StockAdjustment stockAdjustment, CancellationToken cancellationToken = default)
    {
        await _context.StockAdjustments.AddAsync(stockAdjustment, cancellationToken);
    }

    public void Update(StockAdjustment stockAdjustment)
    {
        _context.StockAdjustments.Update(stockAdjustment);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
