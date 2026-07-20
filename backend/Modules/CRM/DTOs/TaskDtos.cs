using backend.Modules.CRM.Enums;

namespace backend.Modules.CRM.DTOs;

public class CreateTaskDto
{
    public Guid? AssignedToId { get; set; }
    public string RelatedEntity { get; set; } = string.Empty;
    public Guid RelatedEntityId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public CrmTaskStatus Status { get; set; }
}

public class UpdateTaskDto : CreateTaskDto
{
}

public class TaskResponseDto : CreateTaskDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
