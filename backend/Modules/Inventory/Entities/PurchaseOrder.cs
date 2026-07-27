using backend.Common;
using backend.Modules.Inventory.Enums;

namespace backend.Modules.Inventory.Entities;

public class PurchaseOrder : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string PONumber { get; set; } = string.Empty; // PO-2026-001
    
    public Guid SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    
    public Guid WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
    
    public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Draft;
    
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime ExpectedDeliveryDate { get; set; }
    
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalAmount { get; set; }
    
    public string? Notes { get; set; }
    public string? TermsAndConditions { get; set; }
    
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    
    public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    public ICollection<GoodsReceipt> GoodsReceipts { get; set; } = new List<GoodsReceipt>();
}

public class PurchaseOrderItem : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public Guid PurchaseOrderId { get; set; }
    public PurchaseOrder? PurchaseOrder { get; set; }
    
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public int QuantityOrdered { get; set; }
    public int QuantityReceived { get; set; } = 0;
    
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    
    public string? Notes { get; set; }
}
