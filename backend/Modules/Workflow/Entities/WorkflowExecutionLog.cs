using backend.Common;
using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.Entities;

public class WorkflowExecutionLog : BaseEntity
{
    public Guid ExecutionId { get; set; }
    public Guid WorkflowId { get; set; }
    public string StepName { get; set; } = string.Empty;
    public string StepType { get; set; } = string.Empty;
    public WorkflowExecutionStatus Status { get; set; } = WorkflowExecutionStatus.Running;
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? FinishedAt { get; set; }
    public string InputJson { get; set; } = "{}";
    public string OutputJson { get; set; } = "{}";
    public string? ErrorMessage { get; set; }

    // Navigation property
    public WorkflowExecution Execution { get; set; } = null!;
}
