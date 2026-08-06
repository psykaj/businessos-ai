using System;
using System.Collections.Generic;

namespace backend.Modules.BusinessIntelligence.DTOs;

/// <summary>
/// Represents the overall business health score evaluated across financial and operational metrics.
/// </summary>
public class BusinessHealthDto
{
    /// <summary>
    /// Gets or sets the aggregate business health score, ranging from 0 to 100.
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Gets or sets the qualitative classification of the health score (e.g., "Excellent", "Good", "Needs Attention", "Critical").
    /// </summary>
    public string Status { get; set; } = "Good";

    /// <summary>
    /// Gets or sets the comprehensive explanation detailing why this score was assigned and highlighting key contributing factors.
    /// </summary>
    public string Explanation { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the score breakdown across key business dimensions (Revenue, Customer Growth, Cash Flow, Inventory, Pending Invoices, Customer Satisfaction).
    /// </summary>
    public Dictionary<string, int> DimensionScores { get; set; } = new();

    /// <summary>
    /// Gets or sets specific commentaries and explanations for each calculated dimension.
    /// </summary>
    public Dictionary<string, string> DimensionExplanations { get; set; } = new();

    /// <summary>
    /// Gets or sets the UTC timestamp when the health score was calculated.
    /// </summary>
    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
}
