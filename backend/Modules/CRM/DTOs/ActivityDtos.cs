using backend.Modules.CRM.Enums;

namespace backend.Modules.CRM.DTOs;

public class CreateActivityDto
{
    public string RelatedEntity { get; set; } = string.Empty;
    public Guid RelatedEntityId { get; set; }
    public ActivityType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class UpdateActivityDto : CreateActivityDto
{
}

public class ActivityResponseDto : CreateActivityDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
