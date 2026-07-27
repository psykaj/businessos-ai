using backend.Modules.Inventory.Entities;

namespace backend.Modules.Inventory.Interfaces;

public interface IWarehouseRepository
{
    Task<Warehouse?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Warehouse>> GetAllAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<Warehouse?> GetPrimaryAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task AddAsync(Warehouse warehouse, CancellationToken cancellationToken = default);
    void Update(Warehouse warehouse);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
