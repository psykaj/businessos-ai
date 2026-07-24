using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Modules.AiAgent.Entities;

public class Conversation : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Active"; // Active, Archived

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
