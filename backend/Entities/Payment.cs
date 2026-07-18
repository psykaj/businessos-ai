using backend.Common;

namespace backend.Entities;

public class Payment : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }
    
    public string PlanName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public Guid? InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }
}
