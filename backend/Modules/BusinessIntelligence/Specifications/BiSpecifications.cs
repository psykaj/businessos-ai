using System;
using System.Linq;
using backend.Entities;
using backend.Modules.CashFlow.Entities;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.Inventory.Entities;
using backend.Modules.Invoices.Entities;

namespace backend.Modules.BusinessIntelligence.Specifications;

/// <summary>
/// Specification to retrieve overdue finance invoices requiring immediate collection focus.
/// </summary>
public sealed class OverdueInvoicesSpecification : BaseSpecification<FinanceInvoice>
{
    /// <summary>
    /// Initializes a new instance of <see cref="OverdueInvoicesSpecification"/> filtering by organization and overdue criteria.
    /// </summary>
    /// <param name="organizationId">The organization identifier.</param>
    public OverdueInvoicesSpecification(Guid organizationId)
        : base(i => i.OrganizationId == organizationId && (i.Status == "Overdue" || (i.DueDate < DateTime.UtcNow && i.Status != "Paid" && i.Status != "Cancelled")))
    {
        ApplyOrderByDescending(i => i.TotalAmount);
    }
}

/// <summary>
/// Specification to retrieve products that are currently below their minimum safety stock thresholds or reorder points.
/// </summary>
public sealed class LowStockProductsSpecification : BaseSpecification<Product>
{
    /// <summary>
    /// Initializes a new instance of <see cref="LowStockProductsSpecification"/> filtering active products below reorder thresholds.
    /// </summary>
    /// <param name="organizationId">The organization identifier.</param>
    public LowStockProductsSpecification(Guid organizationId)
        : base(p => p.OrganizationId == organizationId && p.IsActive && !p.IsArchived && p.StockLevels.Any(s => (s.QuantityOnHand - s.QuantityReserved) <= p.ReorderPoint))
    {
        AddInclude(p => p.StockLevels);
        ApplyOrderBy(p => p.Name);
    }
}

/// <summary>
/// Specification to retrieve active inventory stock levels across all products for an organization.
/// </summary>
public sealed class ActiveProductsWithStockSpecification : BaseSpecification<Product>
{
    /// <summary>
    /// Initializes a new instance of <see cref="ActiveProductsWithStockSpecification"/> for evaluating overall inventory health and valuation.
    /// </summary>
    /// <param name="organizationId">The organization identifier.</param>
    public ActiveProductsWithStockSpecification(Guid organizationId)
        : base(p => p.OrganizationId == organizationId && p.IsActive && !p.IsArchived)
    {
        AddInclude(p => p.StockLevels);
        ApplyOrderBy(p => p.Name);
    }
}

/// <summary>
/// Specification to retrieve customer satisfaction scores that indicate high or critical churn risk.
/// </summary>
public sealed class ChurnRiskCustomersSpecification : BaseSpecification<CustomerSatisfactionScore>
{
    /// <summary>
    /// Initializes a new instance of <see cref="ChurnRiskCustomersSpecification"/> filtering for high and critical churn risk profiles.
    /// </summary>
    /// <param name="organizationId">The organization identifier.</param>
    public ChurnRiskCustomersSpecification(Guid organizationId)
        : base(c => c.OrganizationId == organizationId && (c.ChurnRiskScore == ChurnRiskLevel.High || c.ChurnRiskScore == ChurnRiskLevel.Critical || c.CurrentCsat < 60m || c.CurrentNps < 0m))
    {
        ApplyOrderByDescending(c => c.RepeatComplaintRate);
        ApplyPaging(0, 20); // Top 20 risks
    }
}

/// <summary>
/// Specification to retrieve finance invoices issued within a specified date range for revenue velocity and trend analysis.
/// </summary>
public sealed class RecentRevenueInvoicesSpecification : BaseSpecification<FinanceInvoice>
{
    /// <summary>
    /// Initializes a new instance of <see cref="RecentRevenueInvoicesSpecification"/> filtering invoices within a time horizon.
    /// </summary>
    /// <param name="organizationId">The organization identifier.</param>
    /// <param name="fromDate">The start date of the evaluation window (UTC).</param>
    public RecentRevenueInvoicesSpecification(Guid organizationId, DateTime fromDate)
        : base(i => i.OrganizationId == organizationId && i.IssueDate >= fromDate && i.Status != "Cancelled" && i.Status != "Draft")
    {
        AddInclude(i => i.Items);
        ApplyOrderBy(i => i.IssueDate);
    }
}

/// <summary>
/// Specification to retrieve cash flow entries over a specified recent timeframe for cash flow health scoring.
/// </summary>
public sealed class RecentCashFlowSpecification : BaseSpecification<CashFlowEntry>
{
    /// <summary>
    /// Initializes a new instance of <see cref="RecentCashFlowSpecification"/> filtering cash in/out events.
    /// </summary>
    /// <param name="organizationId">The organization identifier.</param>
    /// <param name="fromDate">The start date of the cash flow window.</param>
    public RecentCashFlowSpecification(Guid organizationId, DateTime fromDate)
        : base(c => c.OrganizationId == organizationId && c.EntryDate >= fromDate)
    {
        ApplyOrderBy(c => c.EntryDate);
    }
}

/// <summary>
/// Specification to retrieve active customers for customer base analysis and repeat purchasing rates.
/// </summary>
public sealed class ActiveCustomersSpecification : BaseSpecification<Customer>
{
    /// <summary>
    /// Initializes a new instance of <see cref="ActiveCustomersSpecification"/> filtering by organization.
    /// </summary>
    /// <param name="organizationId">The organization identifier.</param>
    public ActiveCustomersSpecification(Guid organizationId)
        : base(c => c.OrganizationId == organizationId && !c.IsDeleted)
    {
        ApplyOrderByDescending(c => c.CreatedAt);
    }
}
