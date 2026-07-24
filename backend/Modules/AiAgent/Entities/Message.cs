using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Modules.AiAgent.Entities;

public class Message : BaseEntity
{
    [Required]
    public Guid ConversationId { get; set; }

    public virtual Conversation? Conversation { get; set; }

    [Required]
    [MaxLength(20)]
    public string Role { get; set; } = "User"; // User, Assistant, System

    [Required]
    public string Content { get; set; } = string.Empty;

    public string? ToolInvoked { get; set; }

    public string? ExecutionId { get; set; }
}
