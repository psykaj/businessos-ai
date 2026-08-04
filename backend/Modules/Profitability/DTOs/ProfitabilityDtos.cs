using System;
using System.Collections.Generic;

namespace backend.Modules.Profitability.DTOs;

public class ProfitSnapshotDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTime SnapshotDate { get; set; }
    public string Period { get; set; } = string.Empty;
    public decimal GrossRevenue { get; set; }
    public decimal CostOfGoodsSold { get; set; }
    public decimal GrossProfit { get; set; }
    public decimal GrossMarginPercentage { get; set; }
    public decimal OperatingExpenses { get; set; }
    public decimal NetProfit { get; set; }
    public decimal NetMarginPercentage { get; set; }
    public decimal Ebitda { get; set; }
    public string? ProfitCenterBreakdownJson { get; set; }
}

public class CreateProfitSnapshotRequest
{
    public DateTime SnapshotDate { get; set; } = DateTime.UtcNow;
    public string Period { get; set; } = string.Empty;
    public decimal GrossRevenue { get; set; }
    public decimal CostOfGoodsSold { get; set; }
    public decimal OperatingExpenses { get; set; }
    public decimal Ebitda { get; set; }
    public string? ProfitCenterBreakdownJson { get; set; }
}

public class ProfitCenterItemDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = "Product";
    public decimal Revenue { get; set; }
    public decimal Cost { get; set; }
    public decimal MarginPercentage { get; set; }
    public bool IsLossCenter { get; set; }
}

public class ProfitabilityAnalysisDto
{
    public Guid OrganizationId { get; set; }
    public decimal TotalGrossRevenue { get; set; }
    public decimal TotalCostOfGoodsSold { get; set; }
    public decimal OverallGrossMargin { get; set; }
    public decimal TotalOperatingExpenses { get; set; }
    public decimal OverallNetMargin { get; set; }
    public List<ProfitCenterItemDto> ProfitCenters { get; set; } = new();
    public List<ProfitCenterItemDto> LossCenters { get; set; } = new();
    public List<ProfitSnapshotDto> HistoricalSnapshots { get; set; } = new();
}
