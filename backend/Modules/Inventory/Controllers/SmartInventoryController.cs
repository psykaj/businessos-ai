using backend.Modules.Inventory.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Inventory.Controllers;

[Route("api/inventory/smart")]
public class SmartInventoryController : BaseInventoryController
{
    private readonly SmartInventoryService _smartInventoryService;

    public SmartInventoryController(SmartInventoryService smartInventoryService)
    {
        _smartInventoryService = smartInventoryService;
    }

    [HttpGet("alerts")]
    public async Task<IActionResult> GetStockAlerts(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var alerts = await _smartInventoryService.GetStockAlertsAsync(orgId, cancellationToken);
        return Ok(alerts);
    }

    [HttpGet("reorder-suggestions")]
    public async Task<IActionResult> GetReorderSuggestions(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var suggestions = await _smartInventoryService.GetReorderSuggestionsAsync(orgId, cancellationToken);
        return Ok(suggestions);
    }

    [HttpGet("velocity-report")]
    public async Task<IActionResult> GetVelocityReport([FromQuery] int days = 30, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var report = await _smartInventoryService.GetProductVelocityReportAsync(orgId, days, cancellationToken);
        return Ok(report);
    }

    [HttpGet("dashboard-summary")]
    public async Task<IActionResult> GetDashboardSummary(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var summary = await _smartInventoryService.GetDashboardSummaryAsync(orgId, cancellationToken);
        return Ok(summary);
    }
}
