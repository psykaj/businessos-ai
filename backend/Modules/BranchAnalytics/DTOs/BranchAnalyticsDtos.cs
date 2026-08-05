namespace backend.Modules.BranchAnalytics.DTOs;

public record CalculatePerformanceRequestDto(
    int Year,
    int Month
);

public record BranchPerformanceResponseDto(
    Guid Id,
    Guid BranchId,
    string BranchName,
    int Year,
    int Month,
    decimal TotalRevenue,
    decimal TotalExpenses,
    decimal TotalProfit,
    decimal ProfitMarginPercentage,
    int CustomerCount,
    int EmployeeCount,
    decimal InventoryUtilizationPercentage,
    decimal PerformanceScore,
    int Rank,
    bool IsBestPerformer,
    bool IsLowestPerformer,
    decimal MoMRevenueGrowthPercentage,
    decimal MoMProfitGrowthPercentage
);

public record MonthlyComparisonResponseDto(
    Guid BranchId,
    string BranchName,
    int CurrentYear,
    int CurrentMonth,
    decimal CurrentRevenue,
    decimal PreviousRevenue,
    decimal MoMRevenueGrowth,
    decimal CurrentProfit,
    decimal PreviousProfit,
    decimal MoMProfitGrowth,
    decimal CurrentScore,
    decimal PreviousScore
);

public record PerformanceEngineSummaryDto(
    int TotalBranchesCalculated,
    string? BestPerformingBranchName,
    string? LowestPerformingBranchName,
    decimal AverageInventoryUtilization,
    decimal AverageProfitMargin,
    IEnumerable<BranchPerformanceResponseDto> Performances
);
