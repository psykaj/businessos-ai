using backend.Modules.Inventory.Enums;

namespace backend.Modules.Inventory.DTOs;

public record StockMovementResponseDto(
    Guid Id,
    Guid OrganizationId,
    Guid ProductId,
    string ProductName,
    string ProductSKU,
    Guid? SourceWarehouseId,
    string? SourceWarehouseName,
    Guid? DestinationWarehouseId,
    string? DestinationWarehouseName,
    MovementType MovementType,
    int Quantity,
    string? ReferenceType,
    string? ReferenceId,
    decimal UnitCost,
    string? BatchNumber,
    DateTime? ExpiryDate,
    string? Reason,
    DateTime MovementDate
);

public record StockMovementFilterDto(
    Guid? ProductId,
    Guid? WarehouseId,
    MovementType? MovementType,
    DateTime? StartDate,
    DateTime? EndDate,
    int PageNumber = 1,
    int PageSize = 20
);
