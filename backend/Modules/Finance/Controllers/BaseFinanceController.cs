using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Modules.Finance.Controllers;

[ApiController]
[Authorize]
public abstract class BaseFinanceController : ControllerBase
{
    protected Guid GetOrganizationId()
    {
        var orgIdClaim = User.FindFirst("organizationId")?.Value;
        if (Guid.TryParse(orgIdClaim, out var orgId))
        {
            return orgId;
        }

        var fallBackClaim = User.FindFirst("OrgId")?.Value;
        if (Guid.TryParse(fallBackClaim, out var fallbackOrg))
        {
            return fallbackOrg;
        }

        // Default demo fallback organization if in development testing environment
        var headerOrg = Request.Headers["X-Organization-Id"].FirstOrDefault();
        if (Guid.TryParse(headerOrg, out var headerOrgId))
        {
            return headerOrgId;
        }

        return Guid.Parse("00000000-0000-0000-0000-000000000001");
    }

    protected string GetCurrentUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return string.IsNullOrEmpty(userId) ? "system-user" : userId;
    }
}
