using backend.Modules.Workflow.Entities;

namespace backend.Modules.Workflow.Interfaces;

public interface IActionRepository
{
    Task<List<WorkflowAction>> GetByWorkflowIdAsync(Guid workflowId, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<WorkflowAction> actions, CancellationToken cancellationToken = default);
    Task DeleteByWorkflowIdAsync(Guid workflowId, CancellationToken cancellationToken = default);
}
