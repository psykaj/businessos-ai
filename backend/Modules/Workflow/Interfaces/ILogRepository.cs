using backend.Modules.Workflow.Entities;

namespace backend.Modules.Workflow.Interfaces;

public interface ILogRepository
{
    Task<List<WorkflowExecutionLog>> GetByExecutionIdAsync(Guid executionId, CancellationToken cancellationToken = default);
    Task AddAsync(WorkflowExecutionLog log, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<WorkflowExecutionLog> logs, CancellationToken cancellationToken = default);
}
