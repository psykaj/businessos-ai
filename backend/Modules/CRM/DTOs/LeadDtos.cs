using backend.Modules.CRM.Enums;

namespace backend.Modules.CRM.DTOs;

public class CreateLeadDto
{
    public string Source { get; set; } = string.Empty;
    public LeadStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public decimal EstimatedValue { get; set; }
    public string Tags { get; set; } = string.Empty;
    public Guid? AssignedUserId { get; set; }
}

public class UpdateLeadDto : CreateLeadDto
{
}

public class LeadResponseDto : CreateLeadDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
