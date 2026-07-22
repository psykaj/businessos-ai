using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.DTOs;

namespace backend.Modules.Workflow.Interfaces;

public interface IWorkflowLogService
{
    Task<WorkflowExecutionDto?> GetExecutionByIdAsync(Guid executionId, Guid organizationId, CancellationToken cancellationToken = default);
    Task<WorkflowPagedResult<WorkflowExecutionDto>> GetExecutionsByWorkflowIdAsync(Guid workflowId, Guid organizationId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<WorkflowPagedResult<WorkflowExecutionDto>> GetExecutionsByOrganizationIdAsync(Guid organizationId, int pageNumber, int pageSize, WorkflowExecutionStatus? status, CancellationToken cancellationToken = default);
    Task<List<WorkflowExecutionLogDto>> GetLogsByExecutionIdAsync(Guid executionId, Guid organizationId, CancellationToken cancellationToken = default);
}
