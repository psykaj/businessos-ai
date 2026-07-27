using backend.Common;
using backend.Modules.Inventory.Entities;

namespace backend.Modules.Inventory.Interfaces;

public interface IGoodsReceiptRepository
{
    Task<GoodsReceipt?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<PagedResult<GoodsReceipt>> GetPagedAsync(Guid organizationId, Guid? purchaseOrderId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task AddAsync(GoodsReceipt goodsReceipt, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
