namespace backend.Modules.AiAgent.Executions.DTOs;

public class ExecuteCommandRequestDto
{
    public string Command { get; set; } = string.Empty;
    public Guid? ConversationId { get; set; }
    public bool IsConfirmed { get; set; } = false;
    public string? SpecifiedTool { get; set; }
    public Dictionary<string, object>? Parameters { get; set; }
}

public class ExecutionResponseDto
{
    public Guid ExecutionId { get; set; }
    public Guid OrganizationId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public string ToolInvoked { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // Success, Failed, RequiresConfirmation, Pending
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public string ResultSummary { get; set; } = string.Empty;
    public object? Data { get; set; }
    public bool RequiresConfirmation { get; set; }
    public string? ErrorMessage { get; set; }
    public Guid? ConversationId { get; set; }
}
