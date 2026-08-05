using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Modules.Warehouses.DTOs;
using backend.Modules.Warehouses.Interfaces;

namespace backend.Modules.Warehouses.Controllers;

[ApiController]
[Route("api/v1/branch-warehouses")]
[Authorize]
public class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _service;

    public WarehousesController(IWarehouseService service)
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
    [ProducesResponseType(typeof(IEnumerable<WarehouseResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWarehouses([FromQuery] Guid? branchId, CancellationToken cancellationToken)
    {
        var result = await _service.GetWarehousesAsync(GetOrganizationId(), branchId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(WarehouseResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWarehouseById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetWarehouseByIdAsync(id, GetOrganizationId(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(WarehouseResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.CreateWarehouseAsync(GetOrganizationId(), dto, cancellationToken);
        return CreatedAtAction(nameof(GetWarehouseById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(WarehouseResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateWarehouse(Guid id, [FromBody] UpdateWarehouseDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateWarehouseAsync(id, GetOrganizationId(), dto, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteWarehouse(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteWarehouseAsync(id, GetOrganizationId(), cancellationToken);
        return NoContent();
    }
}
