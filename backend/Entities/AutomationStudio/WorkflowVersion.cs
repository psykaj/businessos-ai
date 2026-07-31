using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Entities;

public class WorkflowVersion : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid WorkflowId { get; set; }

    [Required]
    public int VersionNumber { get; set; }

    [Column(TypeName = "jsonb")]
    public string ConfigurationSnapshot { get; set; } = "{}"; // Snapshot of triggers, conditions, actions at this version

    [MaxLength(200)]
    public string? ChangeLog { get; set; }

    // Navigation properties
    public virtual Organization Organization { get; set; } = null!;
    public virtual AutomationWorkflow Workflow { get; set; } = null!;
}
