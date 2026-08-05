using backend.Modules.RegionalReports.Entities;

namespace backend.Modules.RegionalReports.DTOs;

public record BranchRevenueSummaryDto(
    Guid BranchId,
    string BranchName,
    string? Region,
    decimal TotalRevenue,
    decimal MoMGrowthPercentage
);

public record BranchProfitSummaryDto(
    Guid BranchId,
    string BranchName,
    string? Region,
    decimal TotalRevenue,
    decimal TotalExpenses,
    decimal TotalProfit,
    decimal ProfitMarginPercentage
);

public record BranchInventorySummaryDto(
    Guid BranchId,
    string BranchName,
    int WarehousesCount,
    int TotalStockItems,
    decimal TotalStockValue,
    decimal AverageUtilizationPercentage
);

public record BranchCustomerSummaryDto(
    Guid BranchId,
    string BranchName,
    int TotalCustomers,
    decimal RevenuePerCustomer
);

public record BranchEmployeeSummaryDto(
    Guid BranchId,
    string BranchName,
    int TotalEmployees,
    decimal RevenuePerEmployee
);

public record BranchRankingSummaryDto(
    int Rank,
    Guid BranchId,
    string BranchName,
    string? Region,
    decimal PerformanceScore,
    decimal TotalRevenue,
    decimal TotalProfit,
    bool IsBestPerformer,
    bool IsLowestPerformer
);

public record RegionalOverviewResponseDto(
    Guid Id,
    string Region,
    int Month,
    int Year,
    int TotalBranches,
    decimal TotalRevenue,
    decimal TotalProfit,
    decimal TotalInventoryValue,
    int TotalCustomers,
    int TotalEmployees,
    string TopPerformingBranchName
);
