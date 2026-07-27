namespace backend.Modules.Inventory.DTOs;

public record CreateGoodsReceiptDto(
    Guid PurchaseOrderId,
    string? Notes,
    List<CreateGoodsReceiptItemDto> Items
);

public record CreateGoodsReceiptItemDto(
    Guid PurchaseOrderItemId,
    Guid ProductId,
    int QuantityReceived,
    int QuantityRejected = 0,
    string? RejectionReason = null,
    decimal UnitCost = 0,
    string? BatchNumber = null,
    DateTime? ExpiryDate = null
);

public record GoodsReceiptResponseDto(
    Guid Id,
    Guid OrganizationId,
    string ReceiptNumber,
    Guid PurchaseOrderId,
    string PONumber,
    Guid SupplierId,
    string SupplierName,
    Guid WarehouseId,
    string WarehouseName,
    DateTime ReceiptDate,
    string ReceivedBy,
    string? Notes,
    List<GoodsReceiptItemResponseDto> Items,
    DateTime CreatedAt
);

public record GoodsReceiptItemResponseDto(
    Guid Id,
    Guid PurchaseOrderItemId,
    Guid ProductId,
    string ProductName,
    string ProductSKU,
    int QuantityReceived,
    int QuantityRejected,
    string? RejectionReason,
    decimal UnitCost,
    string? BatchNumber,
    DateTime? ExpiryDate
);
