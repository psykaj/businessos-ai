using backend.Common;

namespace backend.Entities;

public class Subscription : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }
    
    public string PlanId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CurrentPeriodEnd { get; set; }
    public bool CancelAtPeriodEnd { get; set; }
    
    public string? RazorpaySubscriptionId { get; set; }
    public string? RazorpayCustomerId { get; set; }
}
