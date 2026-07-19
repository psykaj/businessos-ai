using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Interfaces;

using backend.Common;

namespace backend.Entities;

public class AIConversation : BaseEntity
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
    public string Provider { get; set; } = "OpenAI"; // OpenAI, Gemini, Claude
    
    [Required]
    [MaxLength(100)]
    public string Model { get; set; } = "gpt-4-turbo";
    
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Active"; // Active, Archived

    public virtual ICollection<AIMessage> Messages { get; set; } = new List<AIMessage>();
}
