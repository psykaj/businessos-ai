using backend.Common;
using backend.Entities;

namespace backend.Modules.CustomerSuccess.Loyalty.Entities;

public class LoyaltyProgram : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }


    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "Active"; // Active, Draft, Archived
    
    public int PointsPerPurchase { get; set; } = 1; // E.g., 1 point per $1 spent or per transaction
    public int MinimumRedemptionPoints { get; set; } = 100;
    public int? PointsExpiryDays { get; set; } = 365;
    public bool IsDefault { get; set; } = true;

    public ICollection<LoyaltyTransaction> Transactions { get; set; } = new List<LoyaltyTransaction>();
}
