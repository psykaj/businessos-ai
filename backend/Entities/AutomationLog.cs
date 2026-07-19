using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using backend.Common;

namespace backend.Entities;

public class AutomationLog : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    [Required]
    public Guid RuleId { get; set; }
    
    public virtual AutomationRule? Rule { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Success"; // Success, Failed
    
    [Required]
    public DateTime TriggeredAt { get; set; } = DateTime.UtcNow;
    
    public string? ErrorMessage { get; set; }
}
