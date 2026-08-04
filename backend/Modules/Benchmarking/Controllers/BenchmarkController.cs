using System;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Modules.Benchmarking.DTOs;
using backend.Modules.Benchmarking.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Benchmarking.Controllers;

[ApiController]
[Route("api/v1/benchmarking")]
[Authorize]
public class BenchmarkController : ControllerBase
{
    private readonly IBenchmarkService _service;

    public BenchmarkController(IBenchmarkService service)
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
    [ProducesResponseType(typeof(BenchmarkComparisonSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary([FromQuery] string industry = "Enterprise SaaS & AI Software")
    {
        var res = await _service.GetComparisonSummaryAsync(GetOrganizationId(), industry);
        return Ok(res);
    }

    [HttpGet("compare")]
    [ProducesResponseType(typeof(backend.Common.PagedResult<BenchmarkMetricDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMetrics([FromQuery] string? comparisonType, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var res = await _service.GetMetricsAsync(GetOrganizationId(), comparisonType, pageNumber, pageSize);
        return Ok(res);
    }

    [HttpPost("metrics")]
    [ProducesResponseType(typeof(BenchmarkMetricDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> RecordMetric([FromBody] CreateBenchmarkMetricRequest request)
    {
        var created = await _service.RecordMetricAsync(GetOrganizationId(), request, User.Identity?.Name);
        return CreatedAtAction(nameof(GetMetrics), new { id = created.Id }, created);
    }

    [HttpDelete("metrics/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteMetric(Guid id)
    {
        var ok = await _service.DeleteMetricAsync(id, GetOrganizationId());
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpGet("export")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportReport()
    {
        var csv = await _service.ExportBenchmarkReportAsync(GetOrganizationId());
        return Ok(new { exportFormat = "CSV", fileName = "industry-benchmark-report.csv", content = csv, generatedAt = DateTime.UtcNow });
    }
}
