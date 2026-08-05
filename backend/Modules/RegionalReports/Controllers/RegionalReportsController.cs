using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Modules.RegionalReports.DTOs;
using backend.Modules.RegionalReports.Interfaces;

namespace backend.Modules.RegionalReports.Controllers;

[ApiController]
[Route("api/v1/regional-reports")]
[Authorize]
public class RegionalReportsController : ControllerBase
{
    private readonly IRegionalReportService _service;

    public RegionalReportsController(IRegionalReportService service)
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

    [HttpGet("revenue-by-branch")]
    [ProducesResponseType(typeof(IEnumerable<BranchRevenueSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRevenueByBranch([FromQuery] int year = 2026, [FromQuery] int month = 8, [FromQuery] string? region = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetRevenueByBranchAsync(GetOrganizationId(), year, month, region, cancellationToken);
        return Ok(result);
    }

    [HttpGet("profit-by-branch")]
    [ProducesResponseType(typeof(IEnumerable<BranchProfitSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfitByBranch([FromQuery] int year = 2026, [FromQuery] int month = 8, [FromQuery] string? region = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetProfitByBranchAsync(GetOrganizationId(), year, month, region, cancellationToken);
        return Ok(result);
    }

    [HttpGet("inventory-by-branch")]
    [ProducesResponseType(typeof(IEnumerable<BranchInventorySummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInventoryByBranch([FromQuery] string? region = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetInventoryByBranchAsync(GetOrganizationId(), region, cancellationToken);
        return Ok(result);
    }

    [HttpGet("customer-count")]
    [ProducesResponseType(typeof(IEnumerable<BranchCustomerSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomerCount([FromQuery] int year = 2026, [FromQuery] int month = 8, [FromQuery] string? region = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCustomerCountByBranchAsync(GetOrganizationId(), year, month, region, cancellationToken);
        return Ok(result);
    }

    [HttpGet("employee-count")]
    [ProducesResponseType(typeof(IEnumerable<BranchEmployeeSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeeCount([FromQuery] int year = 2026, [FromQuery] int month = 8, [FromQuery] string? region = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetEmployeeCountByBranchAsync(GetOrganizationId(), year, month, region, cancellationToken);
        return Ok(result);
    }

    [HttpGet("branch-ranking")]
    [ProducesResponseType(typeof(IEnumerable<BranchRankingSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBranchRanking([FromQuery] int year = 2026, [FromQuery] int month = 8, [FromQuery] string? region = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetBranchRankingAsync(GetOrganizationId(), year, month, region, cancellationToken);
        return Ok(result);
    }
}
