using backend.Common;

namespace backend.Modules.Inventory.Entities;

public class Product : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string SKU { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public string? QRCode { get; set; }
    
    public Guid CategoryId { get; set; }
    public ProductCategory? Category { get; set; }
    
    public string UnitOfMeasure { get; set; } = "PCS"; // PCS, KG, L, BOX, etc.
    
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    
    public int ReorderPoint { get; set; } = 10;
    public int SafetyStock { get; set; } = 5;
    public int ReorderQuantity { get; set; } = 50;
    public int MinimumStockLevel { get; set; } = 5;
    public int MaxStockLevel { get; set; } = 500;
    
    public bool IsArchived { get; set; } = false;
    public bool IsActive { get; set; } = true;
    
    public ICollection<InventoryStock> StockLevels { get; set; } = new List<InventoryStock>();
}
