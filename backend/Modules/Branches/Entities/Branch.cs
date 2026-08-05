using backend.Common;

namespace backend.Modules.Branches.Entities;

public enum BranchStatus
{
    Active = 0,
    Inactive = 1,
    UnderMaintenance = 2,
    ComingSoon = 3,
    Closed = 4
}

public class Branch : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid? LocationId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // e.g., "NY-001", "HQ-LDN"
    public BranchStatus Status { get; set; } = BranchStatus.Active;
    
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? CostCenterCode { get; set; }
    
    // Configuration & Settings stored as JSON string or defaults
    public string WorkingHoursJson { get; set; } = "{\"Monday\":\"09:00-17:00\",\"Tuesday\":\"09:00-17:00\",\"Wednesday\":\"09:00-17:00\",\"Thursday\":\"09:00-17:00\",\"Friday\":\"09:00-17:00\",\"Saturday\":\"Closed\",\"Sunday\":\"Closed\"}";
    public string OperationalSettingsJson { get; set; } = "{\"AllowAutoStockTransfer\":true,\"MaxTransferApprovalAmount\":10000,\"EnablePosSync\":true,\"AutoReorderEnabled\":false}";
    
    public bool IsPrimary { get; set; } = false;
}
