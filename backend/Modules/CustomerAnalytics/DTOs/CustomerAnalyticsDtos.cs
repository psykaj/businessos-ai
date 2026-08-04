using System;
using System.Collections.Generic;

namespace backend.Modules.CustomerAnalytics.DTOs;

public class CustomerPerformanceDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerSegment { get; set; } = string.Empty;
    public decimal LifetimeValue { get; set; }
    public decimal AcquisitionCost { get; set; }
    public decimal LtvToCacRatio { get; set; }
    public int TotalOrders { get; set; }
    public decimal AverageOrderValue { get; set; }
    public decimal RepeatPurchaseRate { get; set; }
    public DateTime FirstPurchaseDate { get; set; }
    public DateTime LastPurchaseDate { get; set; }
    public int DaysInactive { get; set; }
    public decimal ChurnRiskScore { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CreateCustomerPerformanceRequest
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerSegment { get; set; } = "Standard";
    public decimal LifetimeValue { get; set; }
    public decimal AcquisitionCost { get; set; }
    public int TotalOrders { get; set; }
    public decimal AverageOrderValue { get; set; }
    public DateTime LastPurchaseDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Active";
}

public class CustomerAnalyticsSummaryDto
{
    public Guid OrganizationId { get; set; }
    public int TotalCustomersTracked { get; set; }
    public decimal AverageLtv { get; set; }
    public decimal AverageCac { get; set; }
    public decimal OverallLtvToCacRatio { get; set; }
    public decimal OverallRepeatPurchaseRate { get; set; }
    public int InactiveReengagementTargets { get; set; }
    public int UpsellCandidates { get; set; }
    public List<CustomerPerformanceDto> TopLtvCustomers { get; set; } = new();
    public List<CustomerPerformanceDto> AtRiskCustomers { get; set; } = new();
}
