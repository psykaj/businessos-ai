using backend.Common;

namespace backend.Modules.Inventory.Entities;

public class Warehouse : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // e.g. WH-MAIN, WH-NORTH
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? ManagerName { get; set; }
    public string? ManagerPhone { get; set; }
    
    public bool IsActive { get; set; } = true;
    public bool IsPrimary { get; set; } = false;
    
    public ICollection<InventoryStock> StockItems { get; set; } = new List<InventoryStock>();
}
