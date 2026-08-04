using System;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Modules.RevenueAnalytics.DTOs;
using backend.Modules.RevenueAnalytics.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.RevenueAnalytics.Controllers;

[ApiController]
[Route("api/v1/revenue-analytics")]
[Authorize]
public class RevenueAnalyticsController : ControllerBase
{
    private readonly IRevenueAnalyticsService _service;

    public RevenueAnalyticsController(IRevenueAnalyticsService service)
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

    [HttpGet("trends")]
    [ProducesResponseType(typeof(RevenueAnalyticsSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrends()
    {
        var summary = await _service.GetRevenueSummaryAsync(GetOrganizationId());
        return Ok(summary);
    }

    [HttpGet("snapshots")]
    [ProducesResponseType(typeof(backend.Common.PagedResult<RevenueSnapshotDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSnapshots([FromQuery] string? periodType, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _service.GetSnapshotsAsync(GetOrganizationId(), periodType, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpPost("snapshots")]
    [ProducesResponseType(typeof(RevenueSnapshotDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> RecordSnapshot([FromBody] CreateRevenueSnapshotRequest request)
    {
        var created = await _service.RecordSnapshotAsync(GetOrganizationId(), request, User.Identity?.Name);
        return CreatedAtAction(nameof(GetSnapshots), new { id = created.Id }, created);
    }

    [HttpDelete("snapshots/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteSnapshot(Guid id)
    {
        var res = await _service.DeleteSnapshotAsync(id, GetOrganizationId());
        if (!res) return NotFound();
        return NoContent();
    }

    [HttpGet("export")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportReport()
    {
        var csv = await _service.ExportRevenueReportAsync(GetOrganizationId());
        return Ok(new { exportFormat = "CSV", fileName = "revenue-trends.csv", content = csv, generatedAt = DateTime.UtcNow });
    }
}
