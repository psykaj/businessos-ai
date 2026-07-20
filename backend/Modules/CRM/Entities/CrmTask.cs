using backend.Common;
using backend.Entities;
using backend.Modules.CRM.Enums;

namespace backend.Modules.CRM.Entities;

public class CrmTask : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public Guid? AssignedToId { get; set; }
    public User? AssignedTo { get; set; }

    public string RelatedEntity { get; set; } = string.Empty; // "Lead", "Deal", etc.
    public Guid RelatedEntityId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public DateTime? DueDate { get; set; }

    public CrmTaskStatus Status { get; set; } = CrmTaskStatus.Pending;
}
