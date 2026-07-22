using AutoMapper;
using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.DTOs;
using backend.Modules.Workflow.Entities;
using backend.Modules.Workflow.Interfaces;

namespace backend.Modules.Workflow.Services;

public class WorkflowService : IWorkflowService
{
    private readonly IWorkflowRepository _workflowRepository;
    private readonly IExecutionRepository _executionRepository;
    private readonly IWorkflowExecutionEngine _executionEngine;
    private readonly IMapper _mapper;

    public WorkflowService(
        IWorkflowRepository workflowRepository,
        IExecutionRepository executionRepository,
        IWorkflowExecutionEngine executionEngine,
        IMapper mapper)
    {
        _workflowRepository = workflowRepository;
        _executionRepository = executionRepository;
        _executionEngine = executionEngine;
        _mapper = mapper;
    }

    public async Task<WorkflowDto?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var entity = await _workflowRepository.GetByIdAsync(id, organizationId, cancellationToken);
        return entity == null ? null : _mapper.Map<WorkflowDto>(entity);
    }

    public async Task<WorkflowPagedResult<WorkflowDto>> GetPagedAsync(
        Guid organizationId, int pageNumber, int pageSize, string? search, WorkflowStatus? status, CancellationToken cancellationToken = default)
    {
        pageSize = Math.Clamp(pageSize, 1, WorkflowConstants.MaxPageSize);
        pageNumber = Math.Max(pageNumber, 1);

        var items = await _workflowRepository.GetByOrganizationIdAsync(organizationId, pageNumber, pageSize, search, status, cancellationToken);
        var totalCount = await _workflowRepository.GetCountAsync(organizationId, search, status, cancellationToken);

        var dtos = _mapper.Map<List<WorkflowDto>>(items);
        return new WorkflowPagedResult<WorkflowDto>(dtos, totalCount, pageNumber, pageSize);
    }

    public async Task<WorkflowDto> CreateAsync(Guid organizationId, string createdBy, CreateWorkflowDto dto, CancellationToken cancellationToken = default)
    {
        var workflow = new Entities.Workflow
        {
            OrganizationId = organizationId,
            Name = dto.Name,
            Description = dto.Description,
            CreatedBy = createdBy,
            Status = WorkflowStatus.Draft,
            Version = 1
        };

        if (dto.Trigger != null)
        {
            workflow.Trigger = new WorkflowTrigger
            {
                TriggerType = dto.Trigger.TriggerType,
                TriggerConfiguration = dto.Trigger.TriggerConfiguration,
                Enabled = dto.Trigger.Enabled
            };
        }

        if (dto.Actions != null && dto.Actions.Count > 0)
        {
            workflow.Actions = dto.Actions.Select((a, idx) => new WorkflowAction
            {
                ActionType = a.ActionType,
                Configuration = a.Configuration,
                ExecutionOrder = a.ExecutionOrder > 0 ? a.ExecutionOrder : idx + 1
            }).ToList();
        }

        if (dto.Conditions != null && dto.Conditions.Count > 0)
        {
            workflow.Conditions = dto.Conditions.Select(c => new WorkflowCondition
            {
                FieldName = c.FieldName,
                Operator = c.Operator,
                Value = c.Value,
                LogicalOperator = c.LogicalOperator
            }).ToList();
        }

        var created = await _workflowRepository.AddAsync(workflow, cancellationToken);
        return _mapper.Map<WorkflowDto>(created);
    }

    public async Task<WorkflowDto> UpdateAsync(Guid id, Guid organizationId, UpdateWorkflowDto dto, CancellationToken cancellationToken = default)
    {
        var workflow = await _workflowRepository.GetByIdAsync(id, organizationId, cancellationToken)
            ?? throw new KeyNotFoundException($"Workflow with ID {id} not found.");

        workflow.Name = dto.Name;
        workflow.Description = dto.Description;
        workflow.Status = dto.Status;
        workflow.Version += 1;

        if (dto.Trigger != null)
        {
            if (workflow.Trigger == null)
            {
                workflow.Trigger = new WorkflowTrigger { WorkflowId = workflow.Id };
            }
            workflow.Trigger.TriggerType = dto.Trigger.TriggerType;
            workflow.Trigger.TriggerConfiguration = dto.Trigger.TriggerConfiguration;
            workflow.Trigger.Enabled = dto.Trigger.Enabled;
        }

        if (dto.Actions != null)
        {
            workflow.Actions.Clear();
            foreach (var a in dto.Actions)
            {
                workflow.Actions.Add(new WorkflowAction
                {
                    WorkflowId = workflow.Id,
                    ActionType = a.ActionType,
                    Configuration = a.Configuration,
                    ExecutionOrder = a.ExecutionOrder
                });
            }
        }

        if (dto.Conditions != null)
        {
            workflow.Conditions.Clear();
            foreach (var c in dto.Conditions)
            {
                workflow.Conditions.Add(new WorkflowCondition
                {
                    WorkflowId = workflow.Id,
                    FieldName = c.FieldName,
                    Operator = c.Operator,
                    Value = c.Value,
                    LogicalOperator = c.LogicalOperator
                });
            }
        }

        await _workflowRepository.UpdateAsync(workflow, cancellationToken);
        return _mapper.Map<WorkflowDto>(workflow);
    }

    public async Task DeleteAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var workflow = await _workflowRepository.GetByIdAsync(id, organizationId, cancellationToken)
            ?? throw new KeyNotFoundException($"Workflow with ID {id} not found.");

        await _workflowRepository.DeleteAsync(workflow, cancellationToken);
    }

    public async Task<WorkflowExecutionDto> ExecuteManualAsync(
        Guid id, Guid organizationId, string executedBy, ExecuteWorkflowManualRequest request, CancellationToken cancellationToken = default)
    {
        var workflow = await _workflowRepository.GetByIdAsync(id, organizationId, cancellationToken)
            ?? throw new KeyNotFoundException($"Workflow with ID {id} not found.");

        var initialContext = request.TriggerData ?? new Dictionary<string, string>();
        initialContext["ManualExecutedAt"] = DateTime.UtcNow.ToString("O");
        initialContext["ExecutedBy"] = executedBy;

        var execution = await _executionEngine.ExecuteWorkflowAsync(workflow, initialContext, executedBy, cancellationToken);
        return _mapper.Map<WorkflowExecutionDto>(execution);
    }
}
