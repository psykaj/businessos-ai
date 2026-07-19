using Microsoft.AspNetCore.Authorization;

namespace backend.Middleware;

public class RequirePermissionAttribute : AuthorizeAttribute
{
    public const string PolicyPrefix = "RequirePermission:";

    public RequirePermissionAttribute(string permission)
    {
        Permission = permission;
        Policy = $"{PolicyPrefix}{permission}";
    }

    public string Permission { get; }
}
