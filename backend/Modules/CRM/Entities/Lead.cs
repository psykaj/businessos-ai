using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;
using backend.Entities;
using backend.Modules.CRM.Enums;

namespace backend.Modules.CRM.Entities;

public class Lead : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public Guid? AssignedUserId { get; set; }
    public User? AssignedUser { get; set; }

    public string Source { get; set; } = string.Empty;
    public LeadStatus Status { get; set; } = LeadStatus.New;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    
    public string CompanyName { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal EstimatedValue { get; set; }

    public string Tags { get; set; } = string.Empty; // Comma separated tags
}
