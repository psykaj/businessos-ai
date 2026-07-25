using backend.Common;
using backend.Entities;

namespace backend.Modules.CustomerSuccess.Referrals.Entities;

public class Referral : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }


    public Guid ReferrerCustomerId { get; set; }
    public Customer? ReferrerCustomer { get; set; }

    public Guid? ReferredCustomerId { get; set; }
    public Customer? ReferredCustomer { get; set; }

    public string ReferralCode { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, Converted, Rewarded, Expired

    public bool RewardIssued { get; set; } = false;
    public decimal RewardAmount { get; set; } = 0;
    public string? Notes { get; set; }
}
