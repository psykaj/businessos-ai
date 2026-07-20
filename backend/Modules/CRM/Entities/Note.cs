using backend.Common;
using backend.Entities;

namespace backend.Modules.CRM.Entities;

public class Note : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public string RelatedEntity { get; set; } = string.Empty; // "Lead", "Deal", etc.
    public Guid RelatedEntityId { get; set; }

    public string Content { get; set; } = string.Empty;

    public Guid? CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }
}
