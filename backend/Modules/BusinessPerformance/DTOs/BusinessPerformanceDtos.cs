using System;
using System.Collections.Generic;

namespace backend.Modules.BusinessPerformance.DTOs;

public class BusinessMetricDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string MetricType { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal PreviousValue { get; set; }
    public decimal TargetValue { get; set; }
    public decimal PercentageChange { get; set; }
    public string Unit { get; set; } = "USD";
    public string Period { get; set; } = string.Empty;
    public DateTime CalculatedAt { get; set; }
    public string? MetadataJson { get; set; }
}

public class CreateBusinessMetricRequest
{
    public string MetricType { get; set; } = string.Empty;
    public string Category { get; set; } = "Financial";
    public decimal Value { get; set; }
    public decimal PreviousValue { get; set; }
    public decimal TargetValue { get; set; }
    public string Unit { get; set; } = "USD";
    public string Period { get; set; } = string.Empty;
    public string? MetadataJson { get; set; }
}

public class UpdateBusinessMetricRequest
{
    public decimal Value { get; set; }
    public decimal PreviousValue { get; set; }
    public decimal TargetValue { get; set; }
    public string Unit { get; set; } = "USD";
    public string Period { get; set; } = string.Empty;
    public string? MetadataJson { get; set; }
}

public class BusinessPerformanceDashboardDto
{
    public Guid OrganizationId { get; set; }
    public decimal RevenueGrowth { get; set; }
    public decimal GrossProfit { get; set; }
    public decimal NetProfit { get; set; }
    public decimal MonthlyRecurringRevenue { get; set; }
    public decimal AverageOrderValue { get; set; }
    public decimal CustomerLifetimeValue { get; set; }
    public decimal CustomerAcquisitionCost { get; set; }
    public decimal MarketingRoi { get; set; }
    public decimal RepeatPurchaseRate { get; set; }
    public List<string> TopPerformingProducts { get; set; } = new();
    public List<string> LeastPerformingProducts { get; set; } = new();
    public DateTime LastCalculated { get; set; } = DateTime.UtcNow;
}

public class BusinessPerformanceExportDto
{
    public string ExportFormat { get; set; } = "CSV";
    public string FileName { get; set; } = "business-performance-report.csv";
    public string Content { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}
