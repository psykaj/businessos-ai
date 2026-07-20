using backend.Common;
using backend.Entities;

namespace backend.Modules.CRM.Entities;

public class Tag : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#cccccc";
}
