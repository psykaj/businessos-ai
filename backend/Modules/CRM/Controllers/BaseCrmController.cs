using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Modules.CRM.Controllers;

[ApiController]
[Authorize]
public abstract class BaseCrmController : ControllerBase
{
    protected Guid GetOrganizationId()
    {
        var orgIdClaim = User.FindFirst("organizationId")?.Value;
        if (Guid.TryParse(orgIdClaim, out var orgId))
        {
            return orgId;
        }

        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("User not authenticated");
            
        // Usually, OrganizationId should be in the claims for standard multi-tenant
        throw new UnauthorizedAccessException("Organization ID not found in token.");
    }
    
    protected Guid GetCurrentUserId()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdString, out var userId))
        {
            return userId;
        }
        throw new UnauthorizedAccessException("User ID not found in token.");
    }
}
