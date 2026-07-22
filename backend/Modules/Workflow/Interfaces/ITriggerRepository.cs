using backend.Modules.Workflow.Entities;

namespace backend.Modules.Workflow.Interfaces;

public interface ITriggerRepository
{
    Task<WorkflowTrigger?> GetByWorkflowIdAsync(Guid workflowId, CancellationToken cancellationToken = default);
    Task AddAsync(WorkflowTrigger trigger, CancellationToken cancellationToken = default);
    Task UpdateAsync(WorkflowTrigger trigger, CancellationToken cancellationToken = default);
    Task DeleteAsync(WorkflowTrigger trigger, CancellationToken cancellationToken = default);
}
