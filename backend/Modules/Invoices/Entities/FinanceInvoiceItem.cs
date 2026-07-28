using backend.Common;

namespace backend.Modules.Invoices.Entities;

public class FinanceInvoiceItem : BaseEntity
{
    public Guid InvoiceId { get; set; }
    public FinanceInvoice? Invoice { get; set; }
    
    public Guid? ProductId { get; set; }
    public string Description { get; set; } = string.Empty;
    
    public decimal Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
}
