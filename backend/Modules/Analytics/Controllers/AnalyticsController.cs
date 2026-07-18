using backend.Modules.Analytics.DTOs;
using backend.Modules.Analytics.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Modules.Analytics.Controllers;

[ApiController]
[Route("api/analytics")]
[Authorize] // Assuming all analytics need authorization
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsRepository _analyticsRepository;
    private readonly backend.Persistence.ApplicationDbContext _context;

    public AnalyticsController(IAnalyticsRepository analyticsRepository, backend.Persistence.ApplicationDbContext context)
    {
        _analyticsRepository = analyticsRepository;
        _context = context;
    }

    private async Task<Guid> GetOrganizationIdAsync(CancellationToken cancellationToken = default)
    {
        var orgIdClaim = User.FindFirst("OrganizationId")?.Value;
        if (Guid.TryParse(orgIdClaim, out var orgId))
        {
            return orgId;
        }

        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("User not authenticated");

        var user = await _context.Users.FindAsync(new object[] { userId }, cancellationToken);
        if (user != null)
        {
            return user.OrganizationId;
        }

        throw new UnauthorizedAccessException("Organization ID not found in token.");
    }

    [HttpGet("overview")]
    public async Task<ActionResult<AnalyticsOverviewDto>> GetOverview([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, CancellationToken cancellationToken)
    {
        try
        {
            var orgId = await GetOrganizationIdAsync(cancellationToken);
            var overview = await _analyticsRepository.GetOverviewAsync(orgId, startDate, endDate);
            return Ok(overview);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("qrcode/{id}")]
    public async Task<ActionResult<QRPerformanceDto>> GetQRPerformance(Guid id, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, CancellationToken cancellationToken)
    {
        try
        {
            var orgId = await GetOrganizationIdAsync(cancellationToken);
            var performance = await _analyticsRepository.GetQRPerformanceAsync(orgId, id, startDate, endDate);
            if (performance == null) return NotFound("QR Code not found for this organization.");
            return Ok(performance);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("timeline")]
    public async Task<ActionResult<IEnumerable<ScanTimelineDto>>> GetTimeline([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, CancellationToken cancellationToken)
    {
        try
        {
            var orgId = await GetOrganizationIdAsync(cancellationToken);
            var timeline = await _analyticsRepository.GetTimelineAsync(orgId, startDate, endDate);
            return Ok(timeline);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("devices")]
    public async Task<ActionResult<IEnumerable<DeviceAnalyticsDto>>> GetDevices([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, CancellationToken cancellationToken)
    {
        try
        {
            var orgId = await GetOrganizationIdAsync(cancellationToken);
            var devices = await _analyticsRepository.GetDeviceAnalyticsAsync(orgId, startDate, endDate);
            return Ok(devices);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("browsers")]
    public async Task<ActionResult<IEnumerable<BrowserAnalyticsDto>>> GetBrowsers([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, CancellationToken cancellationToken)
    {
        try
        {
            var orgId = await GetOrganizationIdAsync(cancellationToken);
            var browsers = await _analyticsRepository.GetBrowserAnalyticsAsync(orgId, startDate, endDate);
            return Ok(browsers);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("countries")]
    public async Task<ActionResult<IEnumerable<CountryAnalyticsDto>>> GetCountries([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, CancellationToken cancellationToken)
    {
        try
        {
            var orgId = await GetOrganizationIdAsync(cancellationToken);
            var countries = await _analyticsRepository.GetCountryAnalyticsAsync(orgId, startDate, endDate);
            return Ok(countries);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("referrers")]
    public async Task<ActionResult<IEnumerable<ReferrerAnalyticsDto>>> GetReferrers([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, CancellationToken cancellationToken)
    {
        try
        {
            var orgId = await GetOrganizationIdAsync(cancellationToken);
            var referrers = await _analyticsRepository.GetReferrerAnalyticsAsync(orgId, startDate, endDate);
            return Ok(referrers);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("history")]
    public async Task<ActionResult<backend.Common.PagedResult<ScanHistoryDto>>> GetHistory(
        [FromQuery] Guid? qrCodeId,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var orgId = await GetOrganizationIdAsync(cancellationToken);
            var history = await _analyticsRepository.GetHistoryAsync(orgId, qrCodeId, search, page, pageSize, startDate, endDate);
            return Ok(history);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportCSV([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, CancellationToken cancellationToken)
    {
        try
        {
            var orgId = await GetOrganizationIdAsync(cancellationToken);
            var timeline = await _analyticsRepository.GetTimelineAsync(orgId, startDate, endDate);
            
            // Build simple CSV for demonstration
            var csv = new System.Text.StringBuilder();
            csv.AppendLine("Date,ScanCount");
            foreach (var record in timeline)
            {
                csv.AppendLine($"{record.Date:yyyy-MM-dd},{record.ScanCount}");
            }

            return File(System.Text.Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "analytics_export.csv");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}
