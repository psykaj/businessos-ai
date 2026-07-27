using backend.Common;

namespace backend.Modules.Inventory.Entities;

public class Supplier : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // e.g. SUP-001
    public string ContactPerson { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? TaxId { get; set; }
    public string PaymentTerms { get; set; } = "Net 30"; // Net 30, Net 15, Immediate, etc.
    
    public double PerformanceScore { get; set; } = 100.0; // 0 - 100 based on OTD & fulfillment
    public int TotalOrdersCount { get; set; } = 0;
    public int OnTimeDeliveriesCount { get; set; } = 0;
    
    public bool IsActive { get; set; } = true;
    
    public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
