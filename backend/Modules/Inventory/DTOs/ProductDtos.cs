namespace backend.Modules.Inventory.DTOs;

public record CreateProductDto(
    string SKU,
    string Name,
    string? Description,
    string? Barcode,
    string? QRCode,
    Guid CategoryId,
    string UnitOfMeasure,
    decimal CostPrice,
    decimal SellingPrice,
    int ReorderPoint,
    int SafetyStock,
    int ReorderQuantity,
    int MinimumStockLevel,
    int MaxStockLevel
);

public record UpdateProductDto(
    string Name,
    string? Description,
    string? Barcode,
    string? QRCode,
    Guid CategoryId,
    string UnitOfMeasure,
    decimal CostPrice,
    decimal SellingPrice,
    int ReorderPoint,
    int SafetyStock,
    int ReorderQuantity,
    int MinimumStockLevel,
    int MaxStockLevel,
    bool IsActive
);

public record ProductResponseDto(
    Guid Id,
    Guid OrganizationId,
    string SKU,
    string Name,
    string? Description,
    string? Barcode,
    string? QRCode,
    Guid CategoryId,
    string CategoryName,
    string UnitOfMeasure,
    decimal CostPrice,
    decimal SellingPrice,
    int ReorderPoint,
    int SafetyStock,
    int ReorderQuantity,
    int MinimumStockLevel,
    int MaxStockLevel,
    int TotalQuantityOnHand,
    int TotalQuantityReserved,
    int TotalQuantityAvailable,
    bool IsArchived,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record ProductSearchFilterDto(
    string? Query,
    Guid? CategoryId,
    Guid? WarehouseId,
    bool? IsArchived,
    bool? LowStockOnly,
    int PageNumber = 1,
    int PageSize = 20,
    string? SortBy = "Name",
    bool SortDescending = false
);

public record BarcodeLookupDto(
    string Code,
    ProductResponseDto? Product
);
