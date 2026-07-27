using backend.Modules.Inventory.Enums;

namespace backend.Modules.Inventory.DTOs;

public record CreatePurchaseOrderDto(
    Guid SupplierId,
    Guid WarehouseId,
    DateTime ExpectedDeliveryDate,
    decimal ShippingCost,
    string? Notes,
    string? TermsAndConditions,
    List<CreatePurchaseOrderItemDto> Items
);

public record CreatePurchaseOrderItemDto(
    Guid ProductId,
    int QuantityOrdered,
    decimal UnitPrice,
    string? Notes
);

public record UpdatePurchaseOrderDto(
    Guid SupplierId,
    Guid WarehouseId,
    DateTime ExpectedDeliveryDate,
    decimal ShippingCost,
    string? Notes,
    string? TermsAndConditions,
    List<CreatePurchaseOrderItemDto> Items
);

public record PurchaseOrderResponseDto(
    Guid Id,
    Guid OrganizationId,
    string PONumber,
    Guid SupplierId,
    string SupplierName,
    Guid WarehouseId,
    string WarehouseName,
    PurchaseOrderStatus Status,
    DateTime OrderDate,
    DateTime ExpectedDeliveryDate,
    decimal SubTotal,
    decimal TaxAmount,
    decimal ShippingCost,
    decimal TotalAmount,
    string? Notes,
    string? TermsAndConditions,
    string? ApprovedBy,
    DateTime? ApprovedAt,
    List<PurchaseOrderItemResponseDto> Items,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record PurchaseOrderItemResponseDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string ProductSKU,
    int QuantityOrdered,
    int QuantityReceived,
    decimal UnitPrice,
    decimal TotalPrice,
    string? Notes
);

public record PurchaseOrderFilterDto(
    Guid? SupplierId,
    Guid? WarehouseId,
    PurchaseOrderStatus? Status,
    DateTime? StartDate,
    DateTime? EndDate,
    int PageNumber = 1,
    int PageSize = 20
);
