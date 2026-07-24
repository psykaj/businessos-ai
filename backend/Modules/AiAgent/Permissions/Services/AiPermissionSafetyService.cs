using backend.Modules.AiAgent.Context.Models;
using backend.Modules.AiAgent.Permissions.Interfaces;
using backend.Modules.AiAgent.ToolRegistry.Interfaces;

namespace backend.Modules.AiAgent.Permissions.Services;

public class AiPermissionSafetyService : IAiPermissionSafetyService
{
    public Task<PermissionValidationResult> ValidateExecutionAsync(
        ITool tool,
        AiContext context,
        IDictionary<string, object> parameters,
        bool isConfirmed = false,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify Organization Context boundary
        if (context.OrganizationId == Guid.Empty)
        {
            return Task.FromResult(PermissionValidationResult.Denied("Access denied: Invalid or missing OrganizationId. Cross-tenant execution prevented."));
        }

        // 2. Verify User Permissions
        if (tool.RequiredPermissions != null && tool.RequiredPermissions.Length > 0)
        {
            var missingPermissions = tool.RequiredPermissions
                .Where(reqPerm => !context.UserPermissions.Any(userPerm => string.Equals(userPerm, reqPerm, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (missingPermissions.Count > 0 && !context.UserRoles.Contains("Admin"))
            {
                return Task.FromResult(PermissionValidationResult.Denied(
                    $"Access denied: User '{context.UserName}' lacks required permission(s): {string.Join(", ", missingPermissions)}."));
            }
        }

        // 3. Safety Check: Destructive Action Confirmation
        if (tool.IsDestructive && !isConfirmed)
        {
            return Task.FromResult(PermissionValidationResult.NeedsConfirmation(
                $"Confirmation Required: Executing '{tool.Name}' is a potentially destructive action. Please confirm to proceed."));
        }

        return Task.FromResult(PermissionValidationResult.Allowed());
    }
}
