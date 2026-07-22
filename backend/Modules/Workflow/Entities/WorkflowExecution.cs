using backend.Common;
using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.Entities;

public class WorkflowExecution : BaseEntity
{
    public Guid WorkflowId { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? FinishedAt { get; set; }
    public long DurationMs { get; set; }
    public WorkflowExecutionStatus Status { get; set; } = WorkflowExecutionStatus.Pending;
    public string? ErrorMessage { get; set; }
    public string ExecutedBy { get; set; } = "System";
    public string TriggerDataJson { get; set; } = "{}";
    public string ContextDataJson { get; set; } = "{}";

    // Navigation properties
    public Workflow Workflow { get; set; } = null!;
    public ICollection<WorkflowExecutionLog> Logs { get; set; } = new List<WorkflowExecutionLog>();
}
