using backend.Modules.Inventory.Entities;

namespace backend.Modules.Inventory.Interfaces;

public interface ICategoryRepository
{
    Task<ProductCategory?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductCategory>> GetAllAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task AddAsync(ProductCategory category, CancellationToken cancellationToken = default);
    void Update(ProductCategory category);
    void Delete(ProductCategory category);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
