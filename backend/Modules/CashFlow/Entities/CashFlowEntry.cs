using backend.Common;

namespace backend.Modules.CashFlow.Entities;

public class CashFlowEntry : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public DateTime EntryDate { get; set; } = DateTime.UtcNow;
    
    // CashIn, CashOut
    public string Type { get; set; } = "CashIn";
    
    // Sales, Expense, ARCollection, APPayment, Investment, Loan
    public string Category { get; set; } = "Sales";
    
    public decimal Amount { get; set; }
    public Guid? AccountId { get; set; }
    
    // Invoice, Expense, Payment, Bill
    public string? ReferenceType { get; set; }
    public Guid? ReferenceId { get; set; }
    
    public string? Description { get; set; }
}
