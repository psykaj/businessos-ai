using backend.Common;

namespace backend.Modules.Expenses.Entities;

public class Expense : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TaxRate { get; set; }
    public string Currency { get; set; } = "USD";
    
    public Guid ExpenseCategoryId { get; set; }
    public ExpenseCategory? ExpenseCategory { get; set; }
    
    public DateTime ExpenseDate { get; set; } = DateTime.UtcNow;
    public string? VendorName { get; set; }
    
    // Cash, BankTransfer, CreditCard, UPI, Check
    public string PaymentMethod { get; set; } = "BankTransfer";
    
    // Draft, PendingApproval, Approved, Rejected, Paid
    public string Status { get; set; } = "Paid";
    
    public string? ReceiptUrl { get; set; }
    
    public bool IsRecurring { get; set; }
    // Monthly, Quarterly, Yearly
    public string? RecurringInterval { get; set; }
    public DateTime? NextRecurringDate { get; set; }
    
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
}
