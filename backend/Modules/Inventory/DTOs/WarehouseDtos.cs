namespace backend.Modules.Inventory.DTOs;

public record CreateWarehouseDto(
    string Name,
    string Code,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    string? ManagerName,
    string? ManagerPhone,
    bool IsPrimary = false
);

public record UpdateWarehouseDto(
    string Name,
    string Code,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    string? ManagerName,
    string? ManagerPhone,
    bool IsActive,
    bool IsPrimary
);

public record WarehouseResponseDto(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string Code,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    string? ManagerName,
    string? ManagerPhone,
    bool IsActive,
    bool IsPrimary,
    int TotalItemsCount,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
