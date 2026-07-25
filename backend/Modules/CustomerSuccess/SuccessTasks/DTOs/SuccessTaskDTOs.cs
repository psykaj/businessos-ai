namespace backend.Modules.CustomerSuccess.SuccessTasks.DTOs;

public record SuccessTaskDto(
    Guid Id,
    Guid CustomerId,
    string? CustomerName,
    Guid? AssignedUserId,
    string? AssignedUserName,
    string TaskType,
    string Title,
    string? Description,
    DateTime? DueDate,
    string Priority,
    string Status,
    DateTime CreatedAt
);

public record CreateSuccessTaskDto(
    Guid CustomerId,
    Guid? AssignedUserId,
    string TaskType,
    string Title,
    string? Description,
    DateTime? DueDate,
    string Priority
);

public record UpdateSuccessTaskDto(
    Guid? AssignedUserId,
    string Title,
    string? Description,
    DateTime? DueDate,
    string Priority,
    string Status
);
