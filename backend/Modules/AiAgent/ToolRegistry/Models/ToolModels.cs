namespace backend.Modules.AiAgent.ToolRegistry.Models;

public class ToolExecutionContext
{
    public Guid OrganizationId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public List<string> UserRoles { get; set; } = new();
    public List<string> UserPermissions { get; set; } = new();
    public bool IsConfirmed { get; set; } = false;
}

public class ToolResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
    public bool RequiresConfirmation { get; set; } = false;
    public string? ConfirmationMessage { get; set; }

    public static ToolResult Ok(string message, object? data = null) => new()
    {
        Success = true,
        Message = message,
        Data = data
    };

    public static ToolResult Fail(string message) => new()
    {
        Success = false,
        Message = message
    };

    public static ToolResult NeedsConfirmation(string confirmationMessage) => new()
    {
        Success = false,
        RequiresConfirmation = true,
        ConfirmationMessage = confirmationMessage,
        Message = confirmationMessage
    };
}
