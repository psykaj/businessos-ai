using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Modules.Inventory.Controllers;

[ApiController]
[Authorize]
public abstract class BaseInventoryController : ControllerBase
{
    protected Guid GetOrganizationId()
    {
        var orgIdClaim = User.FindFirst("organizationId")?.Value;
        if (Guid.TryParse(orgIdClaim, out var orgId))
        {
            return orgId;
        }

        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out _))
        {
            // Fallback for default multi-tenant context if token includes Guid org claim
            var fallBackClaim = User.FindFirst("OrgId")?.Value;
            if (Guid.TryParse(fallBackClaim, out var fallbackOrg)) return fallbackOrg;
        }
        
        throw new UnauthorizedAccessException("Organization ID not found in token.");
    }

    protected string GetCurrentUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token.");
        }
        return userId;
    }
}
