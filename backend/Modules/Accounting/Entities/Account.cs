using backend.Common;

namespace backend.Modules.Accounting.Entities;

public class Account : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    
    // Asset, Liability, Equity, Revenue, Expense
    public string AccountType { get; set; } = "Asset";
    
    // e.g. Current Asset, Fixed Asset, Operating Expense, Sales Revenue
    public string? SubCategory { get; set; }
    
    public string Currency { get; set; } = "USD";
    public decimal CurrentBalance { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; }
}
