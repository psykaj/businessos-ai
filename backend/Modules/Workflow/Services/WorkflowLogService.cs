using AutoMapper;
using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.DTOs;
using backend.Modules.Workflow.Interfaces;

namespace backend.Modules.Workflow.Services;

public class WorkflowLogService : IWorkflowLogService
{
    private readonly IExecutionRepository _executionRepository;
    private readonly ILogRepository _logRepository;
    private readonly IMapper _mapper;

    public WorkflowLogService(
        IExecutionRepository executionRepository,
        ILogRepository logRepository,
        IMapper mapper)
    {
        _executionRepository = executionRepository;
        _logRepository = logRepository;
        _mapper = mapper;
    }

    public async Task<WorkflowExecutionDto?> GetExecutionByIdAsync(Guid executionId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var execution = await _executionRepository.GetByIdAsync(executionId, organizationId, cancellationToken);
        return execution == null ? null : _mapper.Map<WorkflowExecutionDto>(execution);
    }

    public async Task<WorkflowPagedResult<WorkflowExecutionDto>> GetExecutionsByWorkflowIdAsync(
        Guid workflowId, Guid organizationId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        pageSize = Math.Clamp(pageSize, 1, WorkflowConstants.MaxPageSize);
        pageNumber = Math.Max(pageNumber, 1);

        var items = await _executionRepository.GetByWorkflowIdAsync(workflowId, organizationId, pageNumber, pageSize, cancellationToken);
        var total = await _executionRepository.GetCountByWorkflowIdAsync(workflowId, organizationId, cancellationToken);

        var dtos = _mapper.Map<List<WorkflowExecutionDto>>(items);
        return new WorkflowPagedResult<WorkflowExecutionDto>(dtos, total, pageNumber, pageSize);
    }

    public async Task<WorkflowPagedResult<WorkflowExecutionDto>> GetExecutionsByOrganizationIdAsync(
        Guid organizationId, int pageNumber, int pageSize, WorkflowExecutionStatus? status, CancellationToken cancellationToken = default)
    {
        pageSize = Math.Clamp(pageSize, 1, WorkflowConstants.MaxPageSize);
        pageNumber = Math.Max(pageNumber, 1);

        var items = await _executionRepository.GetByOrganizationIdAsync(organizationId, pageNumber, pageSize, status, cancellationToken);
        var total = await _executionRepository.GetCountByOrganizationIdAsync(organizationId, status, cancellationToken);

        var dtos = _mapper.Map<List<WorkflowExecutionDto>>(items);
        return new WorkflowPagedResult<WorkflowExecutionDto>(dtos, total, pageNumber, pageSize);
    }

    public async Task<List<WorkflowExecutionLogDto>> GetLogsByExecutionIdAsync(Guid executionId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var execution = await _executionRepository.GetByIdAsync(executionId, organizationId, cancellationToken)
            ?? throw new KeyNotFoundException($"Execution with ID {executionId} not found.");

        var logs = await _logRepository.GetByExecutionIdAsync(execution.Id, cancellationToken);
        return _mapper.Map<List<WorkflowExecutionLogDto>>(logs);
    }
}
