using backend.Common;

namespace backend.Modules.Inventory.Entities;

public class GoodsReceipt : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string ReceiptNumber { get; set; } = string.Empty; // GRN-2026-001
    
    public Guid PurchaseOrderId { get; set; }
    public PurchaseOrder? PurchaseOrder { get; set; }
    
    public Guid SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    
    public Guid WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
    
    public DateTime ReceiptDate { get; set; } = DateTime.UtcNow;
    public string ReceivedBy { get; set; } = string.Empty;
    public string? Notes { get; set; }
    
    public ICollection<GoodsReceiptItem> Items { get; set; } = new List<GoodsReceiptItem>();
}

public class GoodsReceiptItem : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public Guid GoodsReceiptId { get; set; }
    public GoodsReceipt? GoodsReceipt { get; set; }
    
    public Guid PurchaseOrderItemId { get; set; }
    public PurchaseOrderItem? PurchaseOrderItem { get; set; }
    
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public int QuantityReceived { get; set; }
    public int QuantityRejected { get; set; } = 0;
    public string? RejectionReason { get; set; }
    
    public decimal UnitCost { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
}
