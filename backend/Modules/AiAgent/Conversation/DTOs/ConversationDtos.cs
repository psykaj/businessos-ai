namespace backend.Modules.AiAgent.Conversation.DTOs;

public class CreateConversationRequestDto
{
    public string Title { get; set; } = "New Business Conversation";
}

public class UpdateConversationRequestDto
{
    public string? Title { get; set; }
    public string? Status { get; set; } // Active, Archived
}

public class ConversationDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int MessageCount { get; set; }
    public List<MessageDto> Messages { get; set; } = new();
}

public class MessageDto
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ToolInvoked { get; set; }
    public string? ExecutionId { get; set; }
    public DateTime CreatedAt { get; set; }
}
