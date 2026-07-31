using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Entities;

public class ExecutionLog : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid WorkflowExecutionId { get; set; }

    [Required]
    [MaxLength(100)]
    public string StepType { get; set; } = string.Empty; // Trigger, Condition, Action

    public Guid? StepId { get; set; } // The ID of the specific trigger, condition, or action

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Success"; // Success, Failed

    [Column(TypeName = "jsonb")]
    public string Message { get; set; } = "{}"; // Additional info or error details

    // Navigation properties
    public virtual Organization Organization { get; set; } = null!;
    public virtual WorkflowExecution WorkflowExecution { get; set; } = null!;
}
