using backend.Common;
using backend.Entities;

namespace backend.Modules.Forms.Entities;

public class FormSubmission : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public Guid FormId { get; set; }
    public Form? Form { get; set; }

    public string SubmittedData { get; set; } = string.Empty; // JSON representing form fields and answers
    
    public string Source { get; set; } = string.Empty; // e.g. Website, Facebook, etc.
    public string Device { get; set; } = string.Empty;
    public string Browser { get; set; } = string.Empty;
    
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    
    public Guid? CRMLeadId { get; set; } // Tracks if a Lead was generated from this submission
}
