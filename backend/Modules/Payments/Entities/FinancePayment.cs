using backend.Common;

namespace backend.Modules.Payments.Entities;

public class FinancePayment : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string PaymentNumber { get; set; } = string.Empty;
    public Guid? InvoiceId { get; set; }
    public Guid? BillId { get; set; }
    public Guid? AccountId { get; set; }
    
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    
    // BankTransfer, Cash, CreditCard, UPI, Check
    public string PaymentMethod { get; set; } = "BankTransfer";
    
    // Incoming, Outgoing
    public string PaymentType { get; set; } = "Incoming";
    
    public string? ReferenceNumber { get; set; }
    public string? Notes { get; set; }
    
    // Pending, Completed, Failed, Refunded
    public string Status { get; set; } = "Completed";
}
