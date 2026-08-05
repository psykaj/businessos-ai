using backend.Common;

namespace backend.Modules.BranchAnalytics.Entities;

public class BranchPerformance : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    
    public int Month { get; set; }
    public int Year { get; set; }
    
    public decimal TotalRevenue { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal TotalProfit { get; set; }
    public decimal ProfitMarginPercentage { get; set; }
    
    public int CustomerCount { get; set; }
    public int EmployeeCount { get; set; }
    public decimal InventoryUtilizationPercentage { get; set; }
    
    public decimal PerformanceScore { get; set; } // Composite score 0-100 based on profitability and growth
    public int Rank { get; set; }
    public bool IsBestPerformer { get; set; }
    public bool IsLowestPerformer { get; set; }
    
    public decimal MoMRevenueGrowthPercentage { get; set; }
    public decimal MoMProfitGrowthPercentage { get; set; }
}
