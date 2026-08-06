using System;

namespace backend.Modules.BusinessIntelligence.DTOs;

/// <summary>
/// Represents an AI-generated actionable recommendation for improving business performance.
/// </summary>
public class RecommendationDto
{
    /// <summary>
    /// Gets or sets the concise title of the recommendation (e.g., "Revenue dropped 12% this week", "5 customers likely to churn").
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed analysis and description of the observed situation.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the urgency level of the recommendation ("High", "Medium", "Low").
    /// </summary>
    public string Priority { get; set; } = "Medium";

    /// <summary>
    /// Gets or sets the functional domain category ("Revenue", "Customer", "Finance", "Inventory", "Growth").
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the specific recommended operational or strategic action to mitigate risk or capture growth.
    /// </summary>
    public string RecommendedAction { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the AI confidence score representing reliability of the prediction (between 0.0 and 1.0 or percentage 0-100).
    /// </summary>
    public decimal ConfidenceScore { get; set; }

    /// <summary>
    /// Gets or sets the projected financial or operational impact if the recommended action is taken.
    /// </summary>
    public string ExpectedBusinessImpact { get; set; } = string.Empty;
}
