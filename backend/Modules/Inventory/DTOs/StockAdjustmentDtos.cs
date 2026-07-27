using backend.Modules.Inventory.Enums;

namespace backend.Modules.Inventory.DTOs;

public record CreateStockAdjustmentDto(
    Guid WarehouseId,
    AdjustmentType AdjustmentType,
    string Reason,
    List<CreateStockAdjustmentItemDto> Items
);

public record CreateStockAdjustmentItemDto(
    Guid ProductId,
    int SystemQuantity,
    int ActualQuantity,
    decimal UnitCost,
    string? Reason
);

public record StockAdjustmentResponseDto(
    Guid Id,
    Guid OrganizationId,
    string AdjustmentNumber,
    Guid WarehouseId,
    string WarehouseName,
    AdjustmentType AdjustmentType,
    AdjustmentStatus Status,
    string Reason,
    string AdjustedBy,
    string? ApprovedBy,
    DateTime? ApprovedAt,
    List<StockAdjustmentItemResponseDto> Items,
    DateTime CreatedAt
);

public record StockAdjustmentItemResponseDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string ProductSKU,
    int SystemQuantity,
    int ActualQuantity,
    int VarianceQuantity,
    decimal UnitCost,
    string? Reason
);
