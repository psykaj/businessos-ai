using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Modules.BranchSettings.DTOs;
using backend.Modules.BranchSettings.Interfaces;

namespace backend.Modules.BranchSettings.Controllers;

[ApiController]
[Route("api/v1/branch-settings")]
[Authorize]
public class BranchSettingsController : ControllerBase
{
    private readonly IBranchSettingsService _service;

    public BranchSettingsController(IBranchSettingsService service)
    {
        _service = service;
    }

    private Guid GetOrganizationId()
    {
        var orgClaim = User.FindFirst("OrganizationId")?.Value ?? User.FindFirst("org_id")?.Value;
        if (Guid.TryParse(orgClaim, out var organizationId))
        {
            return organizationId;
        }
        return Guid.Parse("00000000-0000-0000-0000-000000000001");
    }

    [HttpGet("{branchId:guid}")]
    [ProducesResponseType(typeof(BranchConfigurationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBranchConfiguration(Guid branchId, CancellationToken cancellationToken)
    {
        var result = await _service.GetConfigurationAsync(branchId, GetOrganizationId(), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{branchId:guid}")]
    [ProducesResponseType(typeof(BranchConfigurationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBranchConfiguration(Guid branchId, [FromBody] UpdateBranchConfigurationDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateConfigurationAsync(branchId, GetOrganizationId(), dto, cancellationToken);
        return Ok(result);
    }
}
