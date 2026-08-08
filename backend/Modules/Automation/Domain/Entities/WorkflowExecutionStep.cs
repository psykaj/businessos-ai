using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;
using backend.Modules.Automation.Domain.Enums;

namespace backend.Modules.Automation.Domain.Entities;

public class WorkflowExecutionStep : BaseEntity
{
    [Required]
    public Guid ExecutionId { get; set; }

    [Required]
    public Guid WorkflowStepId { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string Status { get; set; } = WorkflowExecutionStatus.Pending.ToString();

    public DateTime? StartedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }

    [Column(TypeName = "jsonb")]
    public string? Output { get; set; } // JSON response from AI, action result, etc.

    public string? ErrorMessage { get; set; }

    public virtual WorkflowExecution Execution { get; set; } = null!;
    public virtual WorkflowStep WorkflowStep { get; set; } = null!;
}
