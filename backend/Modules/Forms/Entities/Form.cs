using backend.Common;
using backend.Entities;

namespace backend.Modules.Forms.Entities;

public class Form : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Status { get; set; } = "Draft"; // Draft, Published, Archived
    public string Theme { get; set; } = "Default"; 

    public ICollection<FormField> Fields { get; set; } = new List<FormField>();
    public ICollection<FormSubmission> Submissions { get; set; } = new List<FormSubmission>();
}
