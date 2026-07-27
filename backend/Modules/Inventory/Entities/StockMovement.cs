using backend.Common;
using backend.Modules.Inventory.Enums;

namespace backend.Modules.Inventory.Entities;

public class StockMovement : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public Guid? SourceWarehouseId { get; set; }
    public Warehouse? SourceWarehouse { get; set; }
    
    public Guid? DestinationWarehouseId { get; set; }
    public Warehouse? DestinationWarehouse { get; set; }
    
    public MovementType MovementType { get; set; }
    public int Quantity { get; set; }
    
    public string? ReferenceType { get; set; } // PurchaseOrder, GoodsReceipt, StockAdjustment, Manual
    public string? ReferenceId { get; set; }
    
    public decimal UnitCost { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Reason { get; set; }
    public DateTime MovementDate { get; set; } = DateTime.UtcNow;
}
