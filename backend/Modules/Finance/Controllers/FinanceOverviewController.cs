using backend.Modules.Finance.DTOs;
using backend.Modules.Finance.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Finance.Controllers;

[Route("api/finance")]
public class FinanceOverviewController : BaseFinanceController
{
    private readonly IFinanceOverviewService _overviewService;

    public FinanceOverviewController(IFinanceOverviewService overviewService)
    {
        _overviewService = overviewService;
    }

    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview()
    {
        var orgId = GetOrganizationId();
        var overview = await _overviewService.GetOverviewAsync(orgId);
        return Ok(overview);
    }
}
