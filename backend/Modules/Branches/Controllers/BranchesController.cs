using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Modules.Branches.DTOs;
using backend.Modules.Branches.Entities;
using backend.Modules.Branches.Interfaces;

namespace backend.Modules.Branches.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class BranchesController : ControllerBase
{
    private readonly IBranchService _service;

    public BranchesController(IBranchService service)
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

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BranchResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBranches([FromQuery] Guid? locationId, [FromQuery] BranchStatus? status, CancellationToken cancellationToken)
    {
        var result = await _service.GetBranchesAsync(GetOrganizationId(), locationId, status, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BranchResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBranchById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetBranchByIdAsync(id, GetOrganizationId(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BranchResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBranch([FromBody] CreateBranchDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.CreateBranchAsync(GetOrganizationId(), dto, cancellationToken);
        return CreatedAtAction(nameof(GetBranchById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BranchResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBranch(Guid id, [FromBody] UpdateBranchDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateBranchAsync(id, GetOrganizationId(), dto, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBranch(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteBranchAsync(id, GetOrganizationId(), cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(BranchResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBranchStatus(Guid id, [FromBody] UpdateBranchStatusDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateBranchStatusAsync(id, GetOrganizationId(), dto, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/managers")]
    [ProducesResponseType(typeof(BranchManagerResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignManager(Guid id, [FromBody] AssignManagerDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.AssignManagerAsync(id, GetOrganizationId(), dto, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}/managers")]
    [ProducesResponseType(typeof(IEnumerable<BranchManagerResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBranchManagers(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetBranchManagersAsync(id, GetOrganizationId(), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}/managers/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveManager(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        await _service.RemoveManagerAsync(id, userId, GetOrganizationId(), cancellationToken);
        return NoContent();
    }
}
