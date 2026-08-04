using System;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Modules.CustomerAnalytics.DTOs;
using backend.Modules.CustomerAnalytics.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerAnalytics.Controllers;

[ApiController]
[Route("api/v1/customer-analytics")]
[Authorize]
public class CustomerAnalyticsController : ControllerBase
{
    private readonly ICustomerAnalyticsService _service;

    public CustomerAnalyticsController(ICustomerAnalyticsService service)
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
    [ProducesResponseType(typeof(CustomerAnalyticsSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary()
    {
        var sum = await _service.GetSummaryAsync(GetOrganizationId());
        return Ok(sum);
    }

    [HttpGet("performance")]
    [ProducesResponseType(typeof(backend.Common.PagedResult<CustomerPerformanceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPerformances([FromQuery] string? segment, [FromQuery] string? status, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var res = await _service.GetPerformancesAsync(GetOrganizationId(), segment, status, pageNumber, pageSize);
        return Ok(res);
    }

    [HttpGet("ltv-cac")]
    [ProducesResponseType(typeof(CustomerAnalyticsSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLtvCacAnalysis()
    {
        var sum = await _service.GetSummaryAsync(GetOrganizationId());
        return Ok(new { sum.OverallLtvToCacRatio, sum.AverageLtv, sum.AverageCac, sum.TopLtvCustomers });
    }

    [HttpPost("performance")]
    [ProducesResponseType(typeof(CustomerPerformanceDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> RecordPerformance([FromBody] CreateCustomerPerformanceRequest request)
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
        var csv = await _service.ExportCustomerReportAsync(GetOrganizationId());
        return Ok(new { exportFormat = "CSV", fileName = "customer-ltv-cac-report.csv", content = csv, generatedAt = DateTime.UtcNow });
    }
}
