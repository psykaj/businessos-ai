using backend.Modules.Inventory.Entities;

namespace backend.Modules.Inventory.Interfaces;

public interface IStockRepository
{
    Task<InventoryStock?> GetByProductAndWarehouseAsync(Guid productId, Guid warehouseId, Guid organizationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryStock>> GetStockByProductAsync(Guid productId, Guid organizationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryStock>> GetStockByWarehouseAsync(Guid warehouseId, Guid organizationId, CancellationToken cancellationToken = default);
    Task AddAsync(InventoryStock stock, CancellationToken cancellationToken = default);
    void Update(InventoryStock stock);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
