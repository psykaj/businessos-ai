using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Modules.Locations.DTOs;
using backend.Modules.Locations.Interfaces;
using System.Security.Claims;

namespace backend.Modules.Locations.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class LocationsController : ControllerBase
{
    private readonly ILocationService _service;

    public LocationsController(ILocationService service)
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
        // Fallback for demo or development admin token
        return Guid.Parse("00000000-0000-0000-0000-000000000001");
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LocationResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocations([FromQuery] string? region, CancellationToken cancellationToken)
    {
        var result = await _service.GetLocationsAsync(GetOrganizationId(), region, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(LocationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLocationById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetLocationByIdAsync(id, GetOrganizationId(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(LocationResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLocation([FromBody] CreateLocationDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.CreateLocationAsync(GetOrganizationId(), dto, cancellationToken);
        return CreatedAtAction(nameof(GetLocationById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(LocationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLocation(Guid id, [FromBody] UpdateLocationDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateLocationAsync(id, GetOrganizationId(), dto, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLocation(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteLocationAsync(id, GetOrganizationId(), cancellationToken);
        return NoContent();
    }
}
