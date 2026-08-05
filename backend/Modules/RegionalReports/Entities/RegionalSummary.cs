using backend.Common;

namespace backend.Modules.RegionalReports.Entities;

public class RegionalSummary : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public string Region { get; set; } = string.Empty;
    
    public int Month { get; set; }
    public int Year { get; set; }
    
    public int TotalBranches { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalProfit { get; set; }
    public decimal TotalInventoryValue { get; set; }
    
    public int TotalCustomers { get; set; }
    public int TotalEmployees { get; set; }
    
    public string TopPerformingBranchName { get; set; } = string.Empty;
    public Guid? TopPerformingBranchId { get; set; }
}
