using backend.Modules.Workflow.Entities;

namespace backend.Modules.Workflow.Interfaces;

public interface IWorkflowExecutionEngine
{
    Task<WorkflowExecution> ExecuteWorkflowAsync(Entities.Workflow workflow, Dictionary<string, string> initialContext, string executedBy = "System", CancellationToken cancellationToken = default);
}
