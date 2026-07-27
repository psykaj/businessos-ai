using backend.Common;

namespace backend.Modules.Inventory.Entities;

public class InventoryStock : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public Guid WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
    
    public int QuantityOnHand { get; set; } = 0;
    public int QuantityReserved { get; set; } = 0;
    
    // Available = QuantityOnHand - QuantityReserved
    public int QuantityAvailable => QuantityOnHand - QuantityReserved;
    
    public string? LocationBin { get; set; } // e.g. A-12-3
    public int ReorderLevel { get; set; } = 10;
}
