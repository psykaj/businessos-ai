using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Entities;

public class Trigger : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid WorkflowId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Type { get; set; } = string.Empty; // e.g., LeadCreated, WebhookReceived

    [Column(TypeName = "jsonb")]
    public string Configuration { get; set; } = "{}"; // JSON configuration specific to the trigger type

    // Navigation properties
    public virtual Organization Organization { get; set; } = null!;
    public virtual AutomationWorkflow Workflow { get; set; } = null!;
}
