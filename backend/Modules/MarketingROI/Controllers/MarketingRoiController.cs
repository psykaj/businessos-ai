using System;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Modules.MarketingROI.DTOs;
using backend.Modules.MarketingROI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.MarketingROI.Controllers;

[ApiController]
[Route("api/v1/marketing-roi")]
[Authorize]
public class MarketingRoiController : ControllerBase
{
    private readonly IMarketingRoiService _service;

    public MarketingRoiController(IMarketingRoiService service)
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

    [HttpGet("summary")]
    [ProducesResponseType(typeof(MarketingRoiSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary()
    {
        var sum = await _service.GetSummaryAsync(GetOrganizationId());
        return Ok(sum);
    }

    [HttpGet("channels")]
    [ProducesResponseType(typeof(backend.Common.PagedResult<MarketingPerformanceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPerformances([FromQuery] string? channel, [FromQuery] string? period, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var res = await _service.GetPerformancesAsync(GetOrganizationId(), channel, period, pageNumber, pageSize);
        return Ok(res);
    }

    [HttpPost("performance")]
    [ProducesResponseType(typeof(MarketingPerformanceDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> RecordPerformance([FromBody] CreateMarketingPerformanceRequest request)
    {
        var res = await _service.RecordPerformanceAsync(GetOrganizationId(), request, User.Identity?.Name);
        return CreatedAtAction(nameof(GetPerformances), new { id = res.Id }, res);
    }

    [HttpDelete("performance/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeletePerformance(Guid id)
    {
        var ok = await _service.DeletePerformanceAsync(id, GetOrganizationId());
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpGet("export")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportReport()
    {
        var csv = await _service.ExportMarketingReportAsync(GetOrganizationId());
        return Ok(new { exportFormat = "CSV", fileName = "marketing-roi-report.csv", content = csv, generatedAt = DateTime.UtcNow });
    }
}
