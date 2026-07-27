using backend.Common;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.DTOs;

namespace backend.Modules.Inventory.Interfaces;

public interface IStockMovementRepository
{
    Task<StockMovement?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<PagedResult<StockMovement>> GetPagedAsync(Guid organizationId, StockMovementFilterDto filter, CancellationToken cancellationToken = default);
    Task AddAsync(StockMovement movement, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
