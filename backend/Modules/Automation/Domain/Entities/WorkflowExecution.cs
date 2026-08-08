using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;
using backend.Modules.Automation.Domain.Enums;

namespace backend.Modules.Automation.Domain.Entities;

public class WorkflowExecution : BaseEntity
{
    [Required]
    public Guid WorkflowId { get; set; }

    [Required]
    public Guid OrganizationId { get; set; } // Acts as BusinessId

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string Status { get; set; } = WorkflowExecutionStatus.Pending.ToString();

    public DateTime? StartedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }

    public string? ErrorMessage { get; set; }

    [MaxLength(200)]
    public string? TriggeredBy { get; set; } // The event that triggered this

    [Column(TypeName = "jsonb")]
    public string? ExecutionContext { get; set; } // Context data like invoice data, customer id, etc.

    // Navigation properties
    public virtual AiWorkflow Workflow { get; set; } = null!;
    public virtual ICollection<WorkflowExecutionStep> ExecutionSteps { get; set; } = new List<WorkflowExecutionStep>();
}
