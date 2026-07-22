using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.DTOs;

public record CreateWorkflowDto(
    string Name,
    string? Description,
    CreateWorkflowTriggerDto? Trigger,
    List<CreateWorkflowActionDto>? Actions,
    List<CreateWorkflowConditionDto>? Conditions
);

public record UpdateWorkflowDto(
    string Name,
    string? Description,
    WorkflowStatus Status,
    CreateWorkflowTriggerDto? Trigger,
    List<CreateWorkflowActionDto>? Actions,
    List<CreateWorkflowConditionDto>? Conditions
);

public record WorkflowDto(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string? Description,
    WorkflowStatus Status,
    int Version,
    string? CreatedBy,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    WorkflowTriggerDto? Trigger,
    List<WorkflowActionDto> Actions,
    List<WorkflowConditionDto> Conditions
);

public record CreateWorkflowTriggerDto(
    TriggerType TriggerType,
    string TriggerConfiguration,
    bool Enabled = true
);

public record WorkflowTriggerDto(
    Guid Id,
    Guid WorkflowId,
    TriggerType TriggerType,
    string TriggerConfiguration,
    bool Enabled
);

public record CreateWorkflowActionDto(
    ActionType ActionType,
    string Configuration,
    int ExecutionOrder
);

public record WorkflowActionDto(
    Guid Id,
    Guid WorkflowId,
    ActionType ActionType,
    string Configuration,
    int ExecutionOrder,
    List<WorkflowConditionDto> Conditions
);

public record CreateWorkflowConditionDto(
    Guid? WorkflowActionId,
    string FieldName,
    ConditionOperator Operator,
    string Value,
    LogicalOperator LogicalOperator = LogicalOperator.AND
);

public record WorkflowConditionDto(
    Guid Id,
    Guid WorkflowId,
    Guid? WorkflowActionId,
    string FieldName,
    ConditionOperator Operator,
    string Value,
    LogicalOperator LogicalOperator
);

public record ExecuteWorkflowManualRequest(
    Dictionary<string, string>? TriggerData
);

public record WorkflowExecutionDto(
    Guid Id,
    Guid WorkflowId,
    Guid OrganizationId,
    DateTime StartedAt,
    DateTime? FinishedAt,
    long DurationMs,
    WorkflowExecutionStatus Status,
    string? ErrorMessage,
    string ExecutedBy,
    string TriggerDataJson,
    string ContextDataJson,
    List<WorkflowExecutionLogDto> Logs
);

public record WorkflowExecutionLogDto(
    Guid Id,
    Guid ExecutionId,
    Guid WorkflowId,
    string StepName,
    string StepType,
    WorkflowExecutionStatus Status,
    DateTime StartedAt,
    DateTime? FinishedAt,
    string InputJson,
    string OutputJson,
    string? ErrorMessage
);

public record WorkflowPagedResult<T>(
    List<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize
);
