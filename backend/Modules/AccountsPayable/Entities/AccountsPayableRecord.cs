using backend.Common;

namespace backend.Modules.AccountsPayable.Entities;

public class AccountsPayableRecord : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string BillNumber { get; set; } = string.Empty;
    public Guid? SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public Guid? PurchaseOrderId { get; set; }
    
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal BalanceDue { get; set; }
    
    public DateTime DueDate { get; set; }
    
    // Current, Overdue, Paid
    public string Status { get; set; } = "Current";
    public int DaysOverdue { get; set; }
}
