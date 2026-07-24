using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.AiAgent.Controllers;

[ApiController]
[Authorize]
public abstract class BaseAiAgentController : ControllerBase
{
    protected Guid GetOrganizationId()
    {
        var orgIdClaim = User.FindFirst("organizationId")?.Value;
        if (Guid.TryParse(orgIdClaim, out var orgId))
        {
            return orgId;
        }

        // Fallback default organization ID if running in dev/test environment
        var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(subClaim))
        {
            return Guid.Parse("00000000-0000-0000-0000-000000000001");
        }

        throw new UnauthorizedAccessException("Organization ID not found in token context.");
    }

    protected string GetUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            return userId;
        }

        return "system_user";
    }
}
