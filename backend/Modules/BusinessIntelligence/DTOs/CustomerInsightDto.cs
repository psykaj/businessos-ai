using System;
using System.Collections.Generic;

namespace backend.Modules.BusinessIntelligence.DTOs;

/// <summary>
/// Represents analytics and churn predictions for the customer base.
/// </summary>
public class CustomerInsightDto
{
    /// <summary>
    /// Gets or sets the total active customer count.
    /// </summary>
    public int TotalCustomers { get; set; }

    /// <summary>
    /// Gets or sets the customer acquisition growth percentage over the recent period.
    /// </summary>
    public decimal GrowthRate { get; set; }

    /// <summary>
    /// Gets or sets the count of customers identified by AI as high risk of churning.
    /// </summary>
    public int AtRiskCustomerCount { get; set; }

    /// <summary>
    /// Gets or sets the percentage of active repeat customers driving business loyalty.
    /// </summary>
    public decimal RepeatCustomerRate { get; set; }

    /// <summary>
    /// Gets or sets the average customer satisfaction (CSAT) score across the customer base.
    /// </summary>
    public decimal AverageCsatScore { get; set; }

    /// <summary>
    /// Gets or sets the list of high-risk customers requiring immediate retention interventions.
    /// </summary>
    public List<ChurnRiskCustomerDto> TopChurnRisks { get; set; } = new();

    /// <summary>
    /// Gets or sets executive intelligence summary regarding customer retention and loyalty dynamics.
    /// </summary>
    public string Summary { get; set; } = string.Empty;
}

/// <summary>
/// Represents details of a specific customer flagged as high risk for churn.
/// </summary>
public class ChurnRiskCustomerDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the customer.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the display name of the customer.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's contact email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the evaluated risk level (e.g., "High", "Critical").
    /// </summary>
    public string RiskLevel { get; set; } = "High";

    /// <summary>
    /// Gets or sets key indicators or events leading to this churn prediction (e.g., declining purchase frequency, unresolved support complaints).
    /// </summary>
    public string RiskReason { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the recommended engagement strategy to prevent churn.
    /// </summary>
    public string RetentionAction { get; set; } = string.Empty;
}
