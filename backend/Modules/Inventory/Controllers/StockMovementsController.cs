using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Inventory.Controllers;

[Route("api/inventory/stock")]
public class StockMovementsController : BaseInventoryController
{
    private readonly StockMovementService _stockMovementService;

    public StockMovementsController(StockMovementService stockMovementService)
    {
        _stockMovementService = stockMovementService;
    }

    [HttpGet("product/{productId:guid}")]
    public async Task<IActionResult> GetProductStock(Guid productId, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var stock = await _stockMovementService.GetProductStockAsync(productId, orgId, cancellationToken);
        return Ok(stock);
    }

    [HttpGet("movements")]
    public async Task<IActionResult> GetStockMovements([FromQuery] StockMovementFilterDto filter, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _stockMovementService.GetStockMovementsAsync(orgId, filter, cancellationToken);
        return Ok(result);
    }

    [HttpPost("in")]
    public async Task<IActionResult> RecordStockIn([FromBody] StockOperationRequestDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId();
        var result = await _stockMovementService.RecordStockInAsync(orgId, dto, userId, cancellationToken);
        return Ok(result);
    }

    [HttpPost("out")]
    public async Task<IActionResult> RecordStockOut([FromBody] StockOperationRequestDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId();
        var result = await _stockMovementService.RecordStockOutAsync(orgId, dto, userId, cancellationToken);
        return Ok(result);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> TransferStock([FromBody] StockTransferRequestDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId();
        var result = await _stockMovementService.TransferStockAsync(orgId, dto, userId, cancellationToken);
        return Ok(result);
    }

    [HttpPost("reconcile")]
    public async Task<IActionResult> ReconcileStock([FromBody] StockReconciliationRequestDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId();
        await _stockMovementService.ReconcileStockAsync(orgId, dto, userId, cancellationToken);
        return Ok(new { message = "Stock reconciliation completed successfully." });
    }
}
