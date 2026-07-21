using backend.Common;
using backend.Entities;

namespace backend.Modules.LeadCapture.Entities;

public class LeadSource : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public string Name { get; set; } = string.Empty; // e.g. Website, Facebook, Referral
    public string Color { get; set; } = "#000000"; // For UI tags/badges
}
