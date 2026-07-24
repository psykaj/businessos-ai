using backend.Modules.AiAgent.Context.Models;
using backend.Modules.AiAgent.ToolRegistry.Interfaces;

namespace backend.Modules.AiAgent.Permissions.Interfaces;

public class PermissionValidationResult
{
    public bool IsAllowed { get; set; }
    public bool RequiresConfirmation { get; set; }
    public string? Reason { get; set; }

    public static PermissionValidationResult Allowed() => new() { IsAllowed = true };
    public static PermissionValidationResult Denied(string reason) => new() { IsAllowed = false, Reason = reason };
    public static PermissionValidationResult NeedsConfirmation(string reason) => new() { IsAllowed = true, RequiresConfirmation = true, Reason = reason };
}

public interface IAiPermissionSafetyService
{
    Task<PermissionValidationResult> ValidateExecutionAsync(
        ITool tool,
        AiContext context,
        IDictionary<string, object> parameters,
        bool isConfirmed = false,
        CancellationToken cancellationToken = default);
}
