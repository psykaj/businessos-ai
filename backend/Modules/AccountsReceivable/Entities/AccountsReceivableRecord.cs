using backend.Common;

namespace backend.Modules.AccountsReceivable.Entities;

public class AccountsReceivableRecord : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public Guid InvoiceId { get; set; }
    public Guid? CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal BalanceDue { get; set; }
    
    public DateTime DueDate { get; set; }
    
    // Current, Overdue, Paid, WrittenOff
    public string Status { get; set; } = "Current";
    public int DaysOverdue { get; set; }
    
    public DateTime? LastReminderSentAt { get; set; }
}
