namespace backend.Modules.Inventory.DTOs;

public record InventoryStockResponseDto(
    Guid Id,
    Guid OrganizationId,
    Guid ProductId,
    string ProductName,
    string ProductSKU,
    Guid WarehouseId,
    string WarehouseName,
    int QuantityOnHand,
    int QuantityReserved,
    int QuantityAvailable,
    string? LocationBin,
    int ReorderLevel,
    DateTime UpdatedAt
);

public record StockTransferRequestDto(
    Guid ProductId,
    Guid SourceWarehouseId,
    Guid DestinationWarehouseId,
    int Quantity,
    string? Reason,
    string? BatchNumber
);

public record StockOperationRequestDto(
    Guid ProductId,
    Guid WarehouseId,
    int Quantity,
    string Reason,
    string? ReferenceType,
    string? ReferenceId,
    decimal UnitCost,
    string? BatchNumber,
    DateTime? ExpiryDate
);

public record StockReconciliationRequestDto(
    Guid WarehouseId,
    List<StockReconciliationItemDto> Items,
    string Reason
);

public record StockReconciliationItemDto(
    Guid ProductId,
    int ActualQuantity
);
