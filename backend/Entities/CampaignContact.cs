using backend.Common;

namespace backend.Entities;

public class CampaignContact : BaseEntity
{
    public Guid CampaignId { get; set; }
    public Campaign? Campaign { get; set; }

    public Guid ContactId { get; set; }
    public Contact? Contact { get; set; }

    public string Status { get; set; } = "Pending"; // Pending, Sent, Delivered, Read, Failed
}
