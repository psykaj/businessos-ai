using backend.Common;
using backend.Entities;
using backend.Modules.CRM.Enums;

namespace backend.Modules.CRM.Entities;

public class CrmActivity : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public string RelatedEntity { get; set; } = string.Empty; // "Lead", "Contact", "Deal", etc.
    public Guid RelatedEntityId { get; set; }

    public ActivityType Type { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }

    public string Status { get; set; } = string.Empty; // e.g., "Planned", "Completed"
}
