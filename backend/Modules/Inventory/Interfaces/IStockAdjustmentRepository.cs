using backend.Common;
using backend.Modules.Inventory.Entities;

namespace backend.Modules.Inventory.Interfaces;

public interface IStockAdjustmentRepository
{
    Task<StockAdjustment?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<PagedResult<StockAdjustment>> GetPagedAsync(Guid organizationId, Guid? warehouseId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task AddAsync(StockAdjustment stockAdjustment, CancellationToken cancellationToken = default);
    void Update(StockAdjustment stockAdjustment);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
