using backend.Common;
using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Inventory.Repositories;

public class StockMovementRepository : IStockMovementRepository
{
    private readonly ApplicationDbContext _context;

    public StockMovementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StockMovement?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.StockMovements
            .Include(m => m.Product)
            .Include(m => m.SourceWarehouse)
            .Include(m => m.DestinationWarehouse)
            .FirstOrDefaultAsync(m => m.Id == id && m.OrganizationId == organizationId && !m.IsDeleted, cancellationToken);
    }

    public async Task<PagedResult<StockMovement>> GetPagedAsync(Guid organizationId, StockMovementFilterDto filter, CancellationToken cancellationToken = default)
    {
        var query = _context.StockMovements
            .Include(m => m.Product)
            .Include(m => m.SourceWarehouse)
            .Include(m => m.DestinationWarehouse)
            .Where(m => m.OrganizationId == organizationId && !m.IsDeleted);

        if (filter.ProductId.HasValue)
        {
            query = query.Where(m => m.ProductId == filter.ProductId.Value);
        }

        if (filter.WarehouseId.HasValue)
        {
            query = query.Where(m => m.SourceWarehouseId == filter.WarehouseId.Value || m.DestinationWarehouseId == filter.WarehouseId.Value);
        }

        if (filter.MovementType.HasValue)
        {
            query = query.Where(m => m.MovementType == filter.MovementType.Value);
        }

        if (filter.StartDate.HasValue)
        {
            query = query.Where(m => m.MovementDate >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where(m => m.MovementDate <= filter.EndDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(m => m.MovementDate)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<StockMovement>(items, totalCount, filter.PageNumber, filter.PageSize);
    }

    public async Task AddAsync(StockMovement movement, CancellationToken cancellationToken = default)
    {
        await _context.StockMovements.AddAsync(movement, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
