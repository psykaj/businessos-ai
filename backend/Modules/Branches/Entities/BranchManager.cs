using backend.Common;

namespace backend.Modules.Branches.Entities;

public class BranchManager : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid BranchId { get; set; }
    public Guid UserId { get; set; }
    
    public string ManagerName { get; set; } = string.Empty;
    public string ManagerEmail { get; set; } = string.Empty;
    public string? ManagerPhone { get; set; }
    
    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
    public bool CanApproveTransfers { get; set; } = true;
    public decimal MaxTransferApprovalLimit { get; set; } = 25000.00m;
    
    public bool IsActive { get; set; } = true;
}
