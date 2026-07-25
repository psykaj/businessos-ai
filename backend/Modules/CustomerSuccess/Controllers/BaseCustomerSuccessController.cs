using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerSuccess.Controllers;

[ApiController]
[Authorize]
public abstract class BaseCustomerSuccessController : ControllerBase
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
            return userId;
        }

        throw new UnauthorizedAccessException("Organization ID not found in token claims.");
    }

    protected Guid? GetUserId()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdString, out var userId))
        {
            return userId;
        }
        return null;
    }
}
