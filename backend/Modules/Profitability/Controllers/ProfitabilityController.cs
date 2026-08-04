using System;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Modules.Profitability.DTOs;
using backend.Modules.Profitability.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Profitability.Controllers;

[ApiController]
[Route("api/v1/profitability")]
[Authorize]
public class ProfitabilityController : ControllerBase
{
    private readonly IProfitabilityService _service;

    public ProfitabilityController(IProfitabilityService service)
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

    [HttpGet("analysis")]
    [ProducesResponseType(typeof(ProfitabilityAnalysisDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAnalysis()
    {
        var res = await _service.GetAnalysisAsync(GetOrganizationId());
        return Ok(res);
    }

    [HttpGet("snapshots")]
    [ProducesResponseType(typeof(backend.Common.PagedResult<ProfitSnapshotDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSnapshots([FromQuery] string? period, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var res = await _service.GetSnapshotsAsync(GetOrganizationId(), period, pageNumber, pageSize);
        return Ok(res);
    }

    [HttpPost("snapshots")]
    [ProducesResponseType(typeof(ProfitSnapshotDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> RecordSnapshot([FromBody] CreateProfitSnapshotRequest request)
    {
        var created = await _service.RecordSnapshotAsync(GetOrganizationId(), request, User.Identity?.Name);
        return CreatedAtAction(nameof(GetSnapshots), new { id = created.Id }, created);
    }

    [HttpDelete("snapshots/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteSnapshot(Guid id)
    {
        var ok = await _service.DeleteSnapshotAsync(id, GetOrganizationId());
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpGet("export")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportReport()
    {
        var csv = await _service.ExportProfitReportAsync(GetOrganizationId());
        return Ok(new { exportFormat = "CSV", fileName = "profitability-analysis.csv", content = csv, generatedAt = DateTime.UtcNow });
    }
}
