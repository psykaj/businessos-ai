using System;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Modules.ProductAnalytics.DTOs;
using backend.Modules.ProductAnalytics.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.ProductAnalytics.Controllers;

[ApiController]
[Route("api/v1/product-analytics")]
[Authorize]
public class ProductAnalyticsController : ControllerBase
{
    private readonly IProductAnalyticsService _service;

    public ProductAnalyticsController(IProductAnalyticsService service)
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
    [ProducesResponseType(typeof(ProductPerformanceSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary()
    {
        var res = await _service.GetSummaryAsync(GetOrganizationId());
        return Ok(res);
    }

    [HttpGet("performance")]
    [ProducesResponseType(typeof(backend.Common.PagedResult<ProductPerformanceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPerformances([FromQuery] string? period, [FromQuery] string? category, [FromQuery] bool? topPerformers, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var res = await _service.GetPerformancesAsync(GetOrganizationId(), period, category, topPerformers, pageNumber, pageSize);
        return Ok(res);
    }

    [HttpGet("top-performers")]
    [ProducesResponseType(typeof(ProductPerformanceSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopPerformers()
    {
        var summary = await _service.GetSummaryAsync(GetOrganizationId());
        return Ok(summary.TopPerformers);
    }

    [HttpGet("least-performers")]
    [ProducesResponseType(typeof(ProductPerformanceSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLeastPerformers()
    {
        var summary = await _service.GetSummaryAsync(GetOrganizationId());
        return Ok(summary.LeastPerformers);
    }

    [HttpPost("performance")]
    [ProducesResponseType(typeof(ProductPerformanceDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> RecordPerformance([FromBody] CreateProductPerformanceRequest request)
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
        var csv = await _service.ExportProductReportAsync(GetOrganizationId());
        return Ok(new { exportFormat = "CSV", fileName = "product-performance.csv", content = csv, generatedAt = DateTime.UtcNow });
    }
}
