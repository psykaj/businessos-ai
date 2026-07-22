using backend.Common;
using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.Entities;

public class WorkflowAction : BaseEntity
{
    public Guid WorkflowId { get; set; }
    public ActionType ActionType { get; set; }
    public string Configuration { get; set; } = "{}";
    public int ExecutionOrder { get; set; }

    // Navigation properties
    public Workflow Workflow { get; set; } = null!;
    public ICollection<WorkflowCondition> Conditions { get; set; } = new List<WorkflowCondition>();
}
