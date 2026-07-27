using backend.Common;
using backend.Modules.Inventory.DTOs;

namespace backend.Modules.Inventory.Interfaces;

public interface IPurchasingService
{
    Task<PurchaseOrderResponseDto> CreatePurchaseOrderAsync(Guid organizationId, CreatePurchaseOrderDto dto, string userId, CancellationToken cancellationToken = default);
    Task<PurchaseOrderResponseDto> UpdatePurchaseOrderAsync(Guid id, Guid organizationId, UpdatePurchaseOrderDto dto, CancellationToken cancellationToken = default);
    Task<PurchaseOrderResponseDto> ApprovePurchaseOrderAsync(Guid id, Guid organizationId, string userId, CancellationToken cancellationToken = default);
    Task<PurchaseOrderResponseDto?> GetPurchaseOrderByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<PagedResult<PurchaseOrderResponseDto>> GetPurchaseOrdersAsync(Guid organizationId, PurchaseOrderFilterDto filter, CancellationToken cancellationToken = default);
    
    Task<GoodsReceiptResponseDto> ReceiveGoodsAsync(Guid organizationId, CreateGoodsReceiptDto dto, string userId, CancellationToken cancellationToken = default);
    Task<PagedResult<GoodsReceiptResponseDto>> GetGoodsReceiptsAsync(Guid organizationId, Guid? purchaseOrderId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
