using backend.Common;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.DTOs;

namespace backend.Modules.Inventory.Interfaces;

public interface IPurchaseOrderRepository
{
    Task<PurchaseOrder?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<PurchaseOrder?> GetByPONumberAsync(string poNumber, Guid organizationId, CancellationToken cancellationToken = default);
    Task<PagedResult<PurchaseOrder>> GetPagedAsync(Guid organizationId, PurchaseOrderFilterDto filter, CancellationToken cancellationToken = default);
    Task<int> GetCountByStatusAsync(Guid organizationId, Enums.PurchaseOrderStatus status, CancellationToken cancellationToken = default);
    Task AddAsync(PurchaseOrder po, CancellationToken cancellationToken = default);
    void Update(PurchaseOrder po);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
