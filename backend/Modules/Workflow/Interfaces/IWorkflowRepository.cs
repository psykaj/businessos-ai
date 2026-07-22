using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.Interfaces;

public interface IWorkflowRepository
{
    Task<Entities.Workflow?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<Entities.Workflow>> GetByOrganizationIdAsync(Guid organizationId, int pageNumber, int pageSize, string? search, WorkflowStatus? status, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(Guid organizationId, string? search, WorkflowStatus? status, CancellationToken cancellationToken = default);
    Task<List<Entities.Workflow>> GetActiveWorkflowsByTriggerTypeAsync(Guid organizationId, TriggerType triggerType, CancellationToken cancellationToken = default);
    Task<List<Entities.Workflow>> GetActiveWorkflowsByTriggerTypeGlobalAsync(TriggerType triggerType, CancellationToken cancellationToken = default);
    Task<Entities.Workflow> AddAsync(Entities.Workflow workflow, CancellationToken cancellationToken = default);
    Task UpdateAsync(Entities.Workflow workflow, CancellationToken cancellationToken = default);
    Task DeleteAsync(Entities.Workflow workflow, CancellationToken cancellationToken = default);
}
