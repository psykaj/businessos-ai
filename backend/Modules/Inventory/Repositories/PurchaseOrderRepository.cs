using backend.Common;
using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Enums;
using backend.Modules.Inventory.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Inventory.Repositories;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly ApplicationDbContext _context;

    public PurchaseOrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseOrder?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Warehouse)
            .Include(po => po.Items)
                .ThenInclude(i => i.Product)
            .Include(po => po.GoodsReceipts)
            .FirstOrDefaultAsync(po => po.Id == id && po.OrganizationId == organizationId && !po.IsDeleted, cancellationToken);
    }

    public async Task<PurchaseOrder?> GetByPONumberAsync(string poNumber, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Warehouse)
            .Include(po => po.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(po => po.PONumber.ToLower() == poNumber.ToLower() && po.OrganizationId == organizationId && !po.IsDeleted, cancellationToken);
    }

    public async Task<PagedResult<PurchaseOrder>> GetPagedAsync(Guid organizationId, PurchaseOrderFilterDto filter, CancellationToken cancellationToken = default)
    {
        var query = _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Warehouse)
            .Include(po => po.Items)
            .Where(po => po.OrganizationId == organizationId && !po.IsDeleted);

        if (filter.SupplierId.HasValue)
        {
            query = query.Where(po => po.SupplierId == filter.SupplierId.Value);
        }

        if (filter.WarehouseId.HasValue)
        {
            query = query.Where(po => po.WarehouseId == filter.WarehouseId.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(po => po.Status == filter.Status.Value);
        }

        if (filter.StartDate.HasValue)
        {
            query = query.Where(po => po.OrderDate >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where(po => po.OrderDate <= filter.EndDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(po => po.OrderDate)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<PurchaseOrder>(items, totalCount, filter.PageNumber, filter.PageSize);
    }

    public async Task<int> GetCountByStatusAsync(Guid organizationId, PurchaseOrderStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Where(po => po.OrganizationId == organizationId && po.Status == status && !po.IsDeleted)
            .CountAsync(cancellationToken);
    }

    public async Task AddAsync(PurchaseOrder po, CancellationToken cancellationToken = default)
    {
        await _context.PurchaseOrders.AddAsync(po, cancellationToken);
    }

    public void Update(PurchaseOrder po)
    {
        _context.PurchaseOrders.Update(po);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
