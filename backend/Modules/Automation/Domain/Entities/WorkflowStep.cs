using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;
using backend.Modules.Automation.Domain.Enums;

namespace backend.Modules.Automation.Domain.Entities;

public class WorkflowStep : BaseEntity
{
    [Required]
    public Guid WorkflowId { get; set; }

    public int StepOrder { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string StepType { get; set; } = WorkflowStepType.Action.ToString();

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "jsonb")]
    public string? Configuration { get; set; } // JSON string for action config (e.g. Email body, Task details)

    [Column(TypeName = "jsonb")]
    public string? Condition { get; set; } // JSON string for condition config

    public bool IsRequired { get; set; } = true;

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string Status { get; set; } = "Active";

    public virtual AiWorkflow Workflow { get; set; } = null!;
}
