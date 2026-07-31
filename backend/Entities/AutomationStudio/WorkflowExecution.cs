using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Entities;

public class WorkflowExecution : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid WorkflowId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, Failed, Retrying

    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    [Column(TypeName = "jsonb")]
    public string ContextData { get; set; } = "{}"; // Store initial payload or execution context

    // Navigation properties
    public virtual Organization Organization { get; set; } = null!;
    public virtual AutomationWorkflow Workflow { get; set; } = null!;
    public virtual ICollection<ExecutionLog> Logs { get; set; } = new List<ExecutionLog>();
}
