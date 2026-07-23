using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.BusinessIntelligence.Controllers;

[ApiController]
[Authorize]
public abstract class BaseBIController : ControllerBase
{
    protected Guid GetOrganizationId()
    {
        var orgIdClaim = User.FindFirst("organizationId")?.Value;
        if (Guid.TryParse(orgIdClaim, out var orgId))
        {
            return orgId;
        }

        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out var userId))
        {
            // Fallback for default organization token context
            return userId;
        }

        throw new UnauthorizedAccessException("Organization ID not found in token claims.");
    }

    protected string GetCurrentUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return string.IsNullOrEmpty(userId) ? "system" : userId;
    }
}
