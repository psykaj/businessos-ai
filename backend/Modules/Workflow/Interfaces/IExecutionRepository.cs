using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.Entities;

namespace backend.Modules.Workflow.Interfaces;

public interface IExecutionRepository
{
    Task<WorkflowExecution?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<WorkflowExecution>> GetByWorkflowIdAsync(Guid workflowId, Guid organizationId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetCountByWorkflowIdAsync(Guid workflowId, Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<WorkflowExecution>> GetByOrganizationIdAsync(Guid organizationId, int pageNumber, int pageSize, WorkflowExecutionStatus? status, CancellationToken cancellationToken = default);
    Task<int> GetCountByOrganizationIdAsync(Guid organizationId, WorkflowExecutionStatus? status, CancellationToken cancellationToken = default);
    Task<WorkflowExecution> AddAsync(WorkflowExecution execution, CancellationToken cancellationToken = default);
    Task UpdateAsync(WorkflowExecution execution, CancellationToken cancellationToken = default);
}
