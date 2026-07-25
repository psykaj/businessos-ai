using backend.Common;
using backend.Entities;

namespace backend.Modules.CustomerSuccess.CustomerHealth.Entities;

public class CustomerHealth : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }


    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public int HealthScore { get; set; } = 100; // 0 - 100
    public string RiskLevel { get; set; } = "Healthy"; // Healthy, Stable, Needs Attention, High Risk

    public DateTime? LastPurchaseDate { get; set; }
    public DateTime? LastInteractionDate { get; set; }

    public decimal LifetimeValue { get; set; } = 0;
    public decimal OutstandingPayments { get; set; } = 0;

    public int SupportTicketCount { get; set; } = 0;
    public double? SatisfactionRating { get; set; }
    public int ReferralCount { get; set; } = 0;
    public int PurchaseFrequency { get; set; } = 0;

    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
}
