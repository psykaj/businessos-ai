using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Entities;

public class AutomationWorkflow : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Draft"; // Draft, Active, Paused, Archived

    public bool IsActive { get; set; } = false;

    // Navigation properties
    public virtual Organization Organization { get; set; } = null!;
    public virtual ICollection<Trigger> Triggers { get; set; } = new List<Trigger>();
    public virtual ICollection<Condition> Conditions { get; set; } = new List<Condition>();
    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();
    public virtual ICollection<WorkflowVersion> Versions { get; set; } = new List<WorkflowVersion>();
    public virtual ICollection<WorkflowExecution> Executions { get; set; } = new List<WorkflowExecution>();
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
