using System;
using System.Collections.Generic;

namespace backend.Modules.BusinessIntelligence.DTOs;

/// <summary>
/// Represents the comprehensive executive overview across all AI Business Intelligence engines and analytics modules.
/// </summary>
public class DashboardSummaryDto
{
    /// <summary>
    /// Gets or sets the holistic business health score and analytical dimension breakdowns.
    /// </summary>
    public BusinessHealthDto Health { get; set; } = new();

    /// <summary>
    /// Gets or sets intelligence summaries regarding financial performance and revenue momentum.
    /// </summary>
    public RevenueInsightDto Revenue { get; set; } = new();

    /// <summary>
    /// Gets or sets intelligence summaries regarding customer retention and churn risks.
    /// </summary>
    public CustomerInsightDto Customers { get; set; } = new();

    /// <summary>
    /// Gets or sets intelligence summaries regarding supply chain, velocity, and inventory stockouts.
    /// </summary>
    public InventoryInsightDto Inventory { get; set; } = new();

    /// <summary>
    /// Gets or sets the top prioritized AI-generated actionable recommendations for executive decision making.
    /// </summary>
    public List<RecommendationDto> TopRecommendations { get; set; } = new();

    /// <summary>
    /// Gets or sets the total count of outstanding overdue invoices requiring immediate collection focus.
    /// </summary>
    public int OverdueInvoiceCount { get; set; }

    /// <summary>
    /// Gets or sets total outstanding balance owed on overdue invoices.
    /// </summary>
    public decimal OverdueInvoiceTotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when this dashboard intelligence snapshot was generated.
    /// </summary>
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}
