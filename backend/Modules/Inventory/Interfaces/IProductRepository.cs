using backend.Common;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.DTOs;

namespace backend.Modules.Inventory.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<Product?> GetBySKUAsync(string sku, Guid organizationId, CancellationToken cancellationToken = default);
    Task<Product?> GetByBarcodeAsync(string barcode, Guid organizationId, CancellationToken cancellationToken = default);
    Task<PagedResult<Product>> GetPagedAsync(Guid organizationId, ProductSearchFilterDto filter, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetLowStockProductsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetOutOfStockProductsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetOverstockProductsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    void Update(Product product);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
