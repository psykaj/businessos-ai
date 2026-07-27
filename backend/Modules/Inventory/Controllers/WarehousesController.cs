using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Inventory.Controllers;

[Route("api/inventory/warehouses")]
public class WarehousesController : BaseInventoryController
{
    private readonly WarehouseService _warehouseService;

    public WarehousesController(WarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWarehouses(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var warehouses = await _warehouseService.GetAllWarehousesAsync(orgId, cancellationToken);
        return Ok(warehouses);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetWarehouseById(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var warehouse = await _warehouseService.GetWarehouseByIdAsync(id, orgId, cancellationToken);
        if (warehouse == null) return NotFound();
        return Ok(warehouse);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _warehouseService.CreateWarehouseAsync(orgId, dto, cancellationToken);
        return CreatedAtAction(nameof(GetWarehouseById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateWarehouse(Guid id, [FromBody] UpdateWarehouseDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _warehouseService.UpdateWarehouseAsync(id, orgId, dto, cancellationToken);
        return Ok(result);
    }
}
