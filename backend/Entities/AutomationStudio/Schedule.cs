using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Entities;

public class Schedule : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid WorkflowId { get; set; }

    [Required]
    [MaxLength(100)]
    public string CronExpression { get; set; } = string.Empty;

    public DateTime? NextRunAt { get; set; }
    public DateTime? LastRunAt { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Organization Organization { get; set; } = null!;
    public virtual AutomationWorkflow Workflow { get; set; } = null!;
}
