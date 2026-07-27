using backend.Common;
using backend.Modules.Inventory.Enums;

namespace backend.Modules.Inventory.Entities;

public class StockAdjustment : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string AdjustmentNumber { get; set; } = string.Empty; // ADJ-2026-001
    
    public Guid WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
    
    public AdjustmentType AdjustmentType { get; set; } = AdjustmentType.PhysicalCount;
    public AdjustmentStatus Status { get; set; } = AdjustmentStatus.Draft;
    
    public string Reason { get; set; } = string.Empty;
    public string AdjustedBy { get; set; } = string.Empty;
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    
    public ICollection<StockAdjustmentItem> Items { get; set; } = new List<StockAdjustmentItem>();
}

public class StockAdjustmentItem : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public Guid StockAdjustmentId { get; set; }
    public StockAdjustment? StockAdjustment { get; set; }
    
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public int SystemQuantity { get; set; }
    public int ActualQuantity { get; set; }
    public int VarianceQuantity => ActualQuantity - SystemQuantity;
    
    public decimal UnitCost { get; set; }
    public string? Reason { get; set; }
}
