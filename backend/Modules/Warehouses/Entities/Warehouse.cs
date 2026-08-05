using backend.Common;

namespace backend.Modules.Warehouses.Entities;

public enum WarehouseStatus
{
    Operational = 0,
    Full = 1,
    Maintenance = 2,
    Decommissioned = 3
}

public class Warehouse : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid BranchId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // e.g., "WH-BR-01"
    
    public decimal StorageCapacitySqFt { get; set; } = 10000m;
    public decimal CurrentUtilizationPercentage { get; set; } = 0.00m;
    public int TotalStockItemsCount { get; set; } = 0;
    public decimal EstimatedStockValue { get; set; } = 0.00m;
    
    public string? ContactPerson { get; set; }
    public string? ContactPhone { get; set; }
    public WarehouseStatus Status { get; set; } = WarehouseStatus.Operational;
    public bool IsPrimary { get; set; } = false;
}
