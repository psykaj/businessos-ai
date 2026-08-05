using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Modules.BranchAnalytics.DTOs;
using backend.Modules.BranchAnalytics.Interfaces;

namespace backend.Modules.BranchAnalytics.Controllers;

[ApiController]
[Route("api/v1/branch-analytics")]
[Authorize]
public class BranchAnalyticsController : ControllerBase
{
    private readonly IBranchPerformanceEngineService _service;

    public BranchAnalyticsController(IBranchPerformanceEngineService service)
    {
        _service = service;
    }

    private Guid GetOrganizationId()
    {
        var orgClaim = User.FindFirst("OrganizationId")?.Value ?? User.FindFirst("org_id")?.Value;
        if (Guid.TryParse(orgClaim, out var organizationId))
        {
            return organizationId;
        }
        return Guid.Parse("00000000-0000-0000-0000-000000000001");
    }

    [HttpPost("calculate")]
    [ProducesResponseType(typeof(PerformanceEngineSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CalculatePerformance([FromBody] CalculatePerformanceRequestDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.CalculatePerformanceAsync(GetOrganizationId(), dto.Year, dto.Month, cancellationToken);
        return Ok(result);
    }

    [HttpGet("performance")]
    [ProducesResponseType(typeof(PerformanceEngineSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPerformance([FromQuery] int year = 2026, [FromQuery] int month = 8, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPerformanceSummaryAsync(GetOrganizationId(), year, month, cancellationToken);
        return Ok(result);
    }

    [HttpGet("comparison")]
    [ProducesResponseType(typeof(IEnumerable<MonthlyComparisonResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMonthlyComparison([FromQuery] int year = 2026, [FromQuery] int month = 8, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetMonthlyComparisonsAsync(GetOrganizationId(), year, month, cancellationToken);
        return Ok(result);
    }
}
