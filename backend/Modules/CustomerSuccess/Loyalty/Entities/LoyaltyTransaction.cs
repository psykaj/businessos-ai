using backend.Common;
using backend.Entities;

namespace backend.Modules.CustomerSuccess.Loyalty.Entities;

public class LoyaltyTransaction : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }


    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public Guid ProgramId { get; set; }
    public LoyaltyProgram? Program { get; set; }

    public int PointsEarned { get; set; } = 0;
    public int PointsRedeemed { get; set; } = 0;
    public int Balance { get; set; } = 0;

    public string TransactionType { get; set; } = "Earned"; // Earned, Redeemed, Adjusted, Expired
    public string? Description { get; set; }
    public DateTime? ExpiryDate { get; set; }
}
