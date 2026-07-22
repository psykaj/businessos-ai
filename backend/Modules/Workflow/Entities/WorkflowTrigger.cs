using backend.Common;
using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.Entities;

public class WorkflowTrigger : BaseEntity
{
    public Guid WorkflowId { get; set; }
    public TriggerType TriggerType { get; set; }
    public string TriggerConfiguration { get; set; } = "{}";
    public bool Enabled { get; set; } = true;

    // Navigation property
    public Workflow Workflow { get; set; } = null!;
}
