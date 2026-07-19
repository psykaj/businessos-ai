using backend.Modules.Organization.DTOs;
using backend.Modules.Organization.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Organization.Controllers;

[ApiController]
[Route("api/v1/organizations")]
[Authorize] // Enforce JWT Authentication globally for this controller
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationService _organizationService;

    public OrganizationController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [HttpGet("{id}")]
    // TODO: Add RBAC Attribute like [RequirePermission("Organization Settings", "View")]
    public async Task<ActionResult<OrganizationDto>> GetOrganization(Guid id, CancellationToken cancellationToken)
    {
        var result = await _organizationService.GetOrganizationAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    // TODO: Add RBAC Attribute like [RequirePermission("Organization Settings", "Update")]
    public async Task<ActionResult<OrganizationDto>> UpdateOrganization(Guid id, [FromBody] UpdateOrganizationDto dto, CancellationToken cancellationToken)
    {
        var result = await _organizationService.UpdateOrganizationAsync(id, dto, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id}/logo")]
    // TODO: Add RBAC Attribute like [RequirePermission("Organization Settings", "Update")]
    public async Task<ActionResult<string>> UploadLogo(Guid id, IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is empty.");

        var result = await _organizationService.UploadLogoAsync(id, file, cancellationToken);
        return Ok(new { LogoUrl = result });
    }
}
