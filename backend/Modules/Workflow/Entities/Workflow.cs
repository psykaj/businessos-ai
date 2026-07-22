using backend.Common;
using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.Entities;

public class Workflow : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public WorkflowStatus Status { get; set; } = WorkflowStatus.Draft;
    public int Version { get; set; } = 1;

    // Navigation properties
    public WorkflowTrigger? Trigger { get; set; }
    public ICollection<WorkflowAction> Actions { get; set; } = new List<WorkflowAction>();
    public ICollection<WorkflowCondition> Conditions { get; set; } = new List<WorkflowCondition>();
    public ICollection<WorkflowExecution> Executions { get; set; } = new List<WorkflowExecution>();
}
