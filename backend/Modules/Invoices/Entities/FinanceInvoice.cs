using backend.Common;

namespace backend.Modules.Invoices.Entities;

public class FinanceInvoice : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerEmail { get; set; }
    
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(30);
    
    // Draft, Sent, Partial, Paid, Overdue, Cancelled
    public string Status { get; set; } = "Draft";
    
    public decimal SubTotal { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal BalanceDue { get; set; }
    
    public string Currency { get; set; } = "USD";
    public string? Notes { get; set; }
    public string? Terms { get; set; }
    public string? PdfUrl { get; set; }
    
    public ICollection<FinanceInvoiceItem> Items { get; set; } = new List<FinanceInvoiceItem>();
}
