using backend.Common;
using backend.Modules.Inventory.DTOs;

namespace backend.Modules.Inventory.Interfaces;

public interface IInventoryService
{
    Task<ProductResponseDto> CreateProductAsync(Guid organizationId, CreateProductDto dto, CancellationToken cancellationToken = default);
    Task<ProductResponseDto> UpdateProductAsync(Guid id, Guid organizationId, UpdateProductDto dto, CancellationToken cancellationToken = default);
    Task<bool> ArchiveProductAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<ProductResponseDto?> GetProductByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<ProductResponseDto?> GetProductBySKUAsync(string sku, Guid organizationId, CancellationToken cancellationToken = default);
    Task<BarcodeLookupDto> LookupBarcodeAsync(string code, Guid organizationId, CancellationToken cancellationToken = default);
    Task<PagedResult<ProductResponseDto>> SearchProductsAsync(Guid organizationId, ProductSearchFilterDto filter, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<InventoryStockResponseDto>> GetProductStockAsync(Guid productId, Guid organizationId, CancellationToken cancellationToken = default);
    Task<StockMovementResponseDto> RecordStockInAsync(Guid organizationId, StockOperationRequestDto dto, string userId, CancellationToken cancellationToken = default);
    Task<StockMovementResponseDto> RecordStockOutAsync(Guid organizationId, StockOperationRequestDto dto, string userId, CancellationToken cancellationToken = default);
    Task<StockMovementResponseDto> TransferStockAsync(Guid organizationId, StockTransferRequestDto dto, string userId, CancellationToken cancellationToken = default);
    Task<StockAdjustmentResponseDto> CreateAdjustmentAsync(Guid organizationId, CreateStockAdjustmentDto dto, string userId, CancellationToken cancellationToken = default);
    Task<StockAdjustmentResponseDto> ApproveAdjustmentAsync(Guid id, Guid organizationId, string userId, CancellationToken cancellationToken = default);
    Task<bool> ReconcileStockAsync(Guid organizationId, StockReconciliationRequestDto dto, string userId, CancellationToken cancellationToken = default);
    Task<PagedResult<StockMovementResponseDto>> GetStockMovementsAsync(Guid organizationId, StockMovementFilterDto filter, CancellationToken cancellationToken = default);
}
