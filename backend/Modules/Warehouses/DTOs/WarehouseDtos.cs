using backend.Modules.Warehouses.Entities;

namespace backend.Modules.Warehouses.DTOs;

public record CreateWarehouseDto(
    Guid BranchId,
    string Name,
    string Code,
    decimal StorageCapacitySqFt,
    string? ContactPerson,
    string? ContactPhone,
    bool IsPrimary
);

public record UpdateWarehouseDto(
    Guid BranchId,
    string Name,
    string Code,
    decimal StorageCapacitySqFt,
    decimal CurrentUtilizationPercentage,
    int TotalStockItemsCount,
    decimal EstimatedStockValue,
    string? ContactPerson,
    string? ContactPhone,
    WarehouseStatus Status,
    bool IsPrimary
);

public record WarehouseResponseDto(
    Guid Id,
    Guid OrganizationId,
    Guid BranchId,
    string Name,
    string Code,
    decimal StorageCapacitySqFt,
    decimal CurrentUtilizationPercentage,
    int TotalStockItemsCount,
    decimal EstimatedStockValue,
    string? ContactPerson,
    string? ContactPhone,
    WarehouseStatus Status,
    bool IsPrimary,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
