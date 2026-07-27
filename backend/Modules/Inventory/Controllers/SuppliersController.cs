using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Inventory.Controllers;

[Route("api/suppliers")]
public class SuppliersController : BaseInventoryController
{
    private readonly SupplierService _supplierService;

    public SuppliersController(SupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSuppliers([FromQuery] string? query, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var result = await _supplierService.GetSuppliersAsync(orgId, query, pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllSuppliers(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _supplierService.GetAllSuppliersAsync(orgId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSupplierById(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var supplier = await _supplierService.GetSupplierByIdAsync(id, orgId, cancellationToken);
        if (supplier == null) return NotFound();
        return Ok(supplier);
    }

    [HttpGet("{id:guid}/performance")]
    public async Task<IActionResult> GetSupplierPerformance(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var performance = await _supplierService.GetSupplierPerformanceAsync(id, orgId, cancellationToken);
        return Ok(performance);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _supplierService.CreateSupplierAsync(orgId, dto, cancellationToken);
        return CreatedAtAction(nameof(GetSupplierById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateSupplier(Guid id, [FromBody] UpdateSupplierDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _supplierService.UpdateSupplierAsync(id, orgId, dto, cancellationToken);
        return Ok(result);
    }
}
