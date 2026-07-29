using backend.Modules.PublicApi.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.PublicApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(AuthenticationSchemes = "ApiKey,Bearer")]
public abstract class BasePublicApiController : ControllerBase
{
    protected Guid GetOrganizationId()
    {
        // Extract OrganizationId from claims (set by either ApiKey handler or JWT handler)
        var orgClaim = User.Claims.FirstOrDefault(c => c.Type == "OrganizationId");
        if (orgClaim != null && Guid.TryParse(orgClaim.Value, out var orgId))
        {
            return orgId;
        }
        
        throw new UnauthorizedAccessException("Organization context is missing from the authenticated principal.");
    }
}
