using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using backend.Common;

namespace backend.Entities;

public class AIMessage : BaseEntity
{
    [Required]
    public Guid ConversationId { get; set; }
    
    public virtual AIConversation? Conversation { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string Role { get; set; } = string.Empty; // user, assistant, system
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    public int TokenUsage { get; set; } = 0;
}
