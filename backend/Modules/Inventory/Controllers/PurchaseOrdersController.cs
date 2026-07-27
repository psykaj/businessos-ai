using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Inventory.Controllers;

[Route("api/purchasing/orders")]
public class PurchaseOrdersController : BaseInventoryController
{
    private readonly PurchasingService _purchasingService;

    public PurchaseOrdersController(PurchasingService purchasingService)
    {
        _purchasingService = purchasingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPurchaseOrders([FromQuery] PurchaseOrderFilterDto filter, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _purchasingService.GetPurchaseOrdersAsync(orgId, filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPurchaseOrderById(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var po = await _purchasingService.GetPurchaseOrderByIdAsync(id, orgId, cancellationToken);
        if (po == null) return NotFound();
        return Ok(po);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId();
        var result = await _purchasingService.CreatePurchaseOrderAsync(orgId, dto, userId, cancellationToken);
        return CreatedAtAction(nameof(GetPurchaseOrderById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePurchaseOrder(Guid id, [FromBody] UpdatePurchaseOrderDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _purchasingService.UpdatePurchaseOrderAsync(id, orgId, dto, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> ApprovePurchaseOrder(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId();
        var result = await _purchasingService.ApprovePurchaseOrderAsync(id, orgId, userId, cancellationToken);
        return Ok(result);
    }
}
