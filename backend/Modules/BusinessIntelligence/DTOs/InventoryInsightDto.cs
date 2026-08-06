using System;
using System.Collections.Generic;

namespace backend.Modules.BusinessIntelligence.DTOs;

/// <summary>
/// Represents operational intelligence regarding inventory stock levels, velocity, and stockout risks.
/// </summary>
public class InventoryInsightDto
{
    /// <summary>
    /// Gets or sets total number of distinct SKUs tracked in inventory.
    /// </summary>
    public int TotalSkus { get; set; }

    /// <summary>
    /// Gets or sets the number of products currently below safety reorder thresholds ("Inventory running low").
    /// </summary>
    public int LowStockCount { get; set; }

    /// <summary>
    /// Gets or sets the total estimated financial value of goods currently in stock.
    /// </summary>
    public decimal TotalInventoryValue { get; set; }

    /// <summary>
    /// Gets or sets the name and SKU of the product with the highest sales velocity ("Product X sells the fastest").
    /// </summary>
    public string FastestSellingProduct { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the weekly sales velocity of the fastest selling item.
    /// </summary>
    public int FastestSellingVelocity { get; set; }

    /// <summary>
    /// Gets or sets a summary explanation of inventory health and supply chain optimization opportunities.
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a list of inventory items currently experiencing low stock or critical replenishment status.
    /// </summary>
    public List<LowStockItemDto> LowStockItems { get; set; } = new();
}

/// <summary>
/// Represents an inventory item whose current available quantity is below safety levels.
/// </summary>
public class LowStockItemDto
{
    /// <summary>
    /// Gets or sets the product unique identifier.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product display name.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product SKU code.
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity currently available in warehouses.
    /// </summary>
    public int QuantityOnHand { get; set; }

    /// <summary>
    /// Gets or sets the configured minimum reorder point threshold.
    /// </summary>
    public int ReorderPoint { get; set; }

    /// <summary>
    /// Gets or sets the recommended reorder quantity to restore safe stock levels.
    /// </summary>
    public int RecommendedOrderQuantity { get; set; }
}
