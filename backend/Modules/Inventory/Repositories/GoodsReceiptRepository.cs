using backend.Common;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Inventory.Repositories;

public class GoodsReceiptRepository : IGoodsReceiptRepository
{
    private readonly ApplicationDbContext _context;

    public GoodsReceiptRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GoodsReceipt?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.GoodsReceipts
            .Include(gr => gr.PurchaseOrder)
            .Include(gr => gr.Supplier)
            .Include(gr => gr.Warehouse)
            .Include(gr => gr.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(gr => gr.Id == id && gr.OrganizationId == organizationId && !gr.IsDeleted, cancellationToken);
    }

    public async Task<PagedResult<GoodsReceipt>> GetPagedAsync(Guid organizationId, Guid? purchaseOrderId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.GoodsReceipts
            .Include(gr => gr.PurchaseOrder)
            .Include(gr => gr.Supplier)
            .Include(gr => gr.Warehouse)
            .Include(gr => gr.Items)
                .ThenInclude(i => i.Product)
            .Where(gr => gr.OrganizationId == organizationId && !gr.IsDeleted);

        if (purchaseOrderId.HasValue)
        {
            query = query.Where(gr => gr.PurchaseOrderId == purchaseOrderId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(gr => gr.ReceiptDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<GoodsReceipt>(items, totalCount, pageNumber, pageSize);
    }

    public async Task AddAsync(GoodsReceipt goodsReceipt, CancellationToken cancellationToken = default)
    {
        await _context.GoodsReceipts.AddAsync(goodsReceipt, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
