using System;
using System.Collections.Generic;

namespace backend.Modules.RevenueAnalytics.DTOs;

public class RevenueSnapshotDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTime SnapshotDate { get; set; }
    public string PeriodType { get; set; } = string.Empty;
    public decimal TotalRevenue { get; set; }
    public decimal MonthlyRecurringRevenue { get; set; }
    public decimal AnnualRecurringRevenue { get; set; }
    public decimal NewRevenue { get; set; }
    public decimal ExpansionRevenue { get; set; }
    public decimal ContractionRevenue { get; set; }
    public decimal ChurnedRevenue { get; set; }
    public decimal NetNewRevenue { get; set; }
    public string Currency { get; set; } = "USD";
    public string? Notes { get; set; }
}

public class CreateRevenueSnapshotRequest
{
    public DateTime SnapshotDate { get; set; } = DateTime.UtcNow;
    public string PeriodType { get; set; } = "Monthly";
    public decimal TotalRevenue { get; set; }
    public decimal MonthlyRecurringRevenue { get; set; }
    public decimal NewRevenue { get; set; }
    public decimal ExpansionRevenue { get; set; }
    public decimal ContractionRevenue { get; set; }
    public decimal ChurnedRevenue { get; set; }
    public string Currency { get; set; } = "USD";
    public string? Notes { get; set; }
}

public class RevenueTrendDto
{
    public string Period { get; set; } = string.Empty;
    public decimal TotalRevenue { get; set; }
    public decimal Mrr { get; set; }
    public decimal Expansion { get; set; }
    public decimal Churn { get; set; }
    public decimal NetNew { get; set; }
    public decimal GrowthRatePercentage { get; set; }
}

public class RevenueAnalyticsSummaryDto
{
    public Guid OrganizationId { get; set; }
    public decimal CurrentMrr { get; set; }
    public decimal CurrentArr { get; set; }
    public decimal MonthOverMonthMrrGrowth { get; set; }
    public decimal NetRevenueRetentionRate { get; set; }
    public decimal GrossRevenueRetentionRate { get; set; }
    public List<RevenueTrendDto> MonthlyTrends { get; set; } = new();
}
