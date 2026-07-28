using backend.Common;

namespace backend.Modules.Taxes.Entities;

public class TaxRecord : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string TaxName { get; set; } = string.Empty;
    public string TaxCode { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    
    // GST, VAT, SalesTax
    public string TaxType { get; set; } = "GST";
    public string CountryCode { get; set; } = "US";
    
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; }
}
