using backend.Common;

namespace backend.Modules.Documents.Entities;

public class DocumentTemplate : BaseEntity
{
    public Guid OrganizationId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = "General"; // Contract, NDA, Proposal, Invoice, HR
    public string Content { get; set; } = string.Empty; // HTML / Markdown text template
    public string? FieldsJson { get; set; } // Schema of variable fields
    public bool IsActive { get; set; } = true;
}
