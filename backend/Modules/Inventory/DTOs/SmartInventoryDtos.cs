namespace backend.Modules.Inventory.DTOs;

public record ReorderSuggestionDto(
    Guid ProductId,
    string ProductName,
    string SKU,
    int CurrentAvailableStock,
    int ReorderPoint,
    int SafetyStock,
    int SuggestedReorderQuantity,
    decimal EstimatedCost,
    Guid PreferredSupplierId,
    string PreferredSupplierName
);

public record StockAlertDto(
    Guid ProductId,
    string ProductName,
    string SKU,
    string AlertType, // LowStock, OutOfStock, Overstock
    int CurrentStock,
    int Threshold,
    string Recommendation,
    DateTime DetectedAt
);

public record ProductVelocityReportDto(
    Guid ProductId,
    string ProductName,
    string SKU,
    int TotalMovementQuantity,
    decimal TotalValue,
    string Classification, // FastMoving, SlowMoving, Deadstock
    int DaysWithoutMovement
);

public record DashboardInventorySummaryDto(
    int TotalProductsCount,
    int LowStockCount,
    int OutOfStockCount,
    int OverstockCount,
    int PendingPurchaseOrdersCount,
    decimal TotalInventoryValue,
    List<StockAlertDto> RecentAlerts,
    List<ReorderSuggestionDto> TopReorderSuggestions
);
