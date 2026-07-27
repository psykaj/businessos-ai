using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Inventory.Controllers;

[Route("api/purchasing/receipts")]
public class GoodsReceiptsController : BaseInventoryController
{
    private readonly PurchasingService _purchasingService;

    public GoodsReceiptsController(PurchasingService purchasingService)
    {
        _purchasingService = purchasingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetGoodsReceipts([FromQuery] Guid? purchaseOrderId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var result = await _purchasingService.GetGoodsReceiptsAsync(orgId, purchaseOrderId, pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> ReceiveGoods([FromBody] CreateGoodsReceiptDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId();
        var result = await _purchasingService.ReceiveGoodsAsync(orgId, dto, userId, cancellationToken);
        return Ok(result);
    }
}
