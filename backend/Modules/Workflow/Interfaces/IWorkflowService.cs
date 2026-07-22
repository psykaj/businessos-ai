using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.DTOs;

namespace backend.Modules.Workflow.Interfaces;

public interface IWorkflowService
{
    Task<WorkflowDto?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<WorkflowPagedResult<WorkflowDto>> GetPagedAsync(Guid organizationId, int pageNumber, int pageSize, string? search, WorkflowStatus? status, CancellationToken cancellationToken = default);
    Task<WorkflowDto> CreateAsync(Guid organizationId, string createdBy, CreateWorkflowDto dto, CancellationToken cancellationToken = default);
    Task<WorkflowDto> UpdateAsync(Guid id, Guid organizationId, UpdateWorkflowDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<WorkflowExecutionDto> ExecuteManualAsync(Guid id, Guid organizationId, string executedBy, ExecuteWorkflowManualRequest request, CancellationToken cancellationToken = default);
}
