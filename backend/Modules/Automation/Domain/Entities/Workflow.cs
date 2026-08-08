using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;
using backend.Modules.Automation.Domain.Enums;

namespace backend.Modules.Automation.Domain.Entities;

public class AiWorkflow : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; } // Acts as BusinessId

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string TriggerType { get; set; } = WorkflowTriggerType.Manual.ToString();

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string Status { get; set; } = WorkflowStatus.Draft.ToString();

    public int Priority { get; set; } = 0;

    public bool IsActive { get; set; } = false;

    public bool RequiresApproval { get; set; } = false;

    // Navigation properties
    public virtual ICollection<WorkflowStep> Steps { get; set; } = new List<WorkflowStep>();
    public virtual ICollection<WorkflowExecution> Executions { get; set; } = new List<WorkflowExecution>();
}
