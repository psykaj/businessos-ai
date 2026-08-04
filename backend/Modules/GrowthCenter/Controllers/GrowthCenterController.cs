using System;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Modules.GrowthCenter.DTOs;
using backend.Modules.GrowthCenter.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.GrowthCenter.Controllers;

[ApiController]
[Route("api/v1/growth-center")]
[Authorize]
public class GrowthCenterController : ControllerBase
{
    private readonly IGrowthCenterService _service;

    public GrowthCenterController(IGrowthCenterService service)
    {
        _service = service;
    }

    private Guid GetOrganizationId()
    {
        var orgClaim = User.FindFirst("organizationId")?.Value 
            ?? User.FindFirst("OrganizationId")?.Value 
            ?? User.FindFirst(ClaimTypes.GroupSid)?.Value;
        if (Guid.TryParse(orgClaim, out var orgId))
            return orgId;
        return Guid.Parse("00000000-0000-0000-0000-000000000001");
    }

    [HttpGet("overview")]
    [ProducesResponseType(typeof(GrowthCenterOverviewDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOverview([FromQuery] bool forceRefresh = false)
    {
        var res = await _service.GetOverviewAsync(GetOrganizationId(), forceRefresh);
        return Ok(res);
    }

    [HttpGet("action-plan")]
    [ProducesResponseType(typeof(ActionPlanDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActionPlan()
    {
        var plan = await _service.GetActionPlanAsync(GetOrganizationId());
        return Ok(plan);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(GrowthCenterOverviewDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshAll([FromBody] GrowthCenterRefreshRequest request)
    {
        var res = await _service.RefreshAllEnginesAsync(GetOrganizationId());
        return Ok(res);
    }

    [HttpGet("export")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportReport()
    {
        var text = await _service.ExportMasterGrowthReportAsync(GetOrganizationId());
        return Ok(new { exportFormat = "TEXT", fileName = "master-growth-intelligence-report.txt", content = text, generatedAt = DateTime.UtcNow });
    }
}
