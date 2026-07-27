using backend.Common;
using backend.Modules.Inventory.Entities;

namespace backend.Modules.Inventory.Interfaces;

public interface ISupplierRepository
{
    Task<Supplier?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<Supplier?> GetByCodeAsync(string code, Guid organizationId, CancellationToken cancellationToken = default);
    Task<PagedResult<Supplier>> GetPagedAsync(Guid organizationId, string? query, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Supplier>> GetAllAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task AddAsync(Supplier supplier, CancellationToken cancellationToken = default);
    void Update(Supplier supplier);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
