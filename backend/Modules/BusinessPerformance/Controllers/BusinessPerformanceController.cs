using System;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Modules.BusinessPerformance.DTOs;
using backend.Modules.BusinessPerformance.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.BusinessPerformance.Controllers;

[ApiController]
[Route("api/v1/business-performance")]
[Authorize]
public class BusinessPerformanceController : ControllerBase
{
    private readonly IBusinessPerformanceService _service;

    public BusinessPerformanceController(IBusinessPerformanceService service)
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
        return Guid.Parse("00000000-0000-0000-0000-000000000001"); // Default enterprise demo org
    }

    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(BusinessPerformanceDashboardDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboard()
    {
        var dashboard = await _service.GetDashboardAsync(GetOrganizationId());
        return Ok(dashboard);
    }

    [HttpGet("metrics")]
    [ProducesResponseType(typeof(backend.Common.PagedResult<BusinessMetricDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMetrics([FromQuery] string? metricType, [FromQuery] string? category, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _service.GetMetricsAsync(GetOrganizationId(), metricType, category, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpPost("metrics")]
    [ProducesResponseType(typeof(BusinessMetricDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateMetric([FromBody] CreateBusinessMetricRequest request)
    {
        var created = await _service.CreateMetricAsync(GetOrganizationId(), request, User.Identity?.Name);
        return CreatedAtAction(nameof(GetMetrics), new { id = created.Id }, created);
    }

    [HttpPut("metrics/{id:guid}")]
    [ProducesResponseType(typeof(BusinessMetricDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateMetric(Guid id, [FromBody] UpdateBusinessMetricRequest request)
    {
        var updated = await _service.UpdateMetricAsync(id, GetOrganizationId(), request, User.Identity?.Name);
        return Ok(updated);
    }

    [HttpDelete("metrics/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteMetric(Guid id)
    {
        var result = await _service.DeleteMetricAsync(id, GetOrganizationId());
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPost("calculate")]
    [ProducesResponseType(typeof(BusinessPerformanceDashboardDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecalculateEngine()
    {
        var dashboard = await _service.CalculateAndRefreshEngineAsync(GetOrganizationId());
        return Ok(dashboard);
    }

    [HttpGet("export")]
    [ProducesResponseType(typeof(BusinessPerformanceExportDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportReport([FromQuery] string format = "CSV")
    {
        var export = await _service.ExportPerformanceReportAsync(GetOrganizationId(), format);
        return Ok(export);
    }
}
