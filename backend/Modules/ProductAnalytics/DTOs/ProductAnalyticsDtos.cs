using System;
using System.Collections.Generic;

namespace backend.Modules.ProductAnalytics.DTOs;

public class ProductPerformanceDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductSku { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public int UnitsSold { get; set; }
    public decimal RevenueGenerated { get; set; }
    public decimal UnitCost { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalProfit { get; set; }
    public decimal GrossMarginPercentage { get; set; }
    public decimal ReturnRatePercentage { get; set; }
    public decimal InventoryTurnoverRate { get; set; }
    public bool IsTopPerformer { get; set; }
    public bool IsLeastPerformer { get; set; }
    public string? RecommendationNotes { get; set; }
}

public class CreateProductPerformanceRequest
{
    public Guid ProductId { get; set; }
    public string ProductSku { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public int UnitsSold { get; set; }
    public decimal UnitCost { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ReturnRatePercentage { get; set; }
    public string? RecommendationNotes { get; set; }
}

public class ProductPerformanceSummaryDto
{
    public Guid OrganizationId { get; set; }
    public int TotalProductsTracked { get; set; }
    public decimal TotalProductRevenue { get; set; }
    public decimal AverageProductMargin { get; set; }
    public List<ProductPerformanceDto> TopPerformers { get; set; } = new();
    public List<ProductPerformanceDto> LeastPerformers { get; set; } = new();
}
