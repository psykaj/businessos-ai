using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Interfaces;

using backend.Common;

namespace backend.Entities;

public class AutomationRule : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Trigger { get; set; } = string.Empty; // e.g., SubscriptionPurchased
    
    [Column(TypeName = "jsonb")]
    public string Conditions { get; set; } = "[]"; // JSON array of condition objects
    
    [Required]
    [Column(TypeName = "jsonb")]
    public string Actions { get; set; } = "[]"; // JSON array of action objects
    
    public bool IsEnabled { get; set; } = true;
    
    public virtual ICollection<AutomationLog> Logs { get; set; } = new List<AutomationLog>();
}
