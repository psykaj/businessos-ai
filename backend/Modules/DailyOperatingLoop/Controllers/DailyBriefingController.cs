using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.DailyOperatingLoop.DTOs;
using backend.Modules.DailyOperatingLoop.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.DailyOperatingLoop.Controllers;

[ApiController]
[Route("api/daily-briefing")]
[Authorize]
public class DailyBriefingController : ControllerBase
{
    private readonly IDailyBriefingService _briefingService;

    public DailyBriefingController(IDailyBriefingService briefingService)
    {
        _briefingService = briefingService;
    }

    [HttpGet]
    public async Task<ActionResult<DailyBusinessBriefingDto>> GetTodayBriefing(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var briefing = await _briefingService.GetTodayBriefingAsync(orgId, cancellationToken);
        
        if (briefing == null) return NotFound("No briefing found for today. Generate one first.");
        
        return Ok(briefing);
    }

    [HttpGet("{date}")]
    public async Task<ActionResult<DailyBusinessBriefingDto>> GetBriefingByDate([FromRoute] DateTime date, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var briefing = await _briefingService.GetBriefingByDateAsync(orgId, date, cancellationToken);
        
        if (briefing == null) return NotFound($"No briefing found for {date.ToShortDateString()}.");
        
        return Ok(briefing);
    }

    [HttpPost("generate")]
    public async Task<ActionResult<DailyBusinessBriefingDto>> GenerateBriefing([FromQuery] bool forceRegeneration = false, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var briefing = await _briefingService.GenerateBriefingAsync(orgId, DateTime.UtcNow, forceRegeneration, cancellationToken);
        
        return Ok(briefing);
    }

    [HttpGet("today")]
    public async Task<ActionResult<DailyBusinessBriefingDto>> GetTodayBriefingAlternative(CancellationToken cancellationToken)
    {
        return await GetTodayBriefing(cancellationToken);
    }

    [HttpPost("{id}/refresh")]
    public async Task<ActionResult<DailyBusinessBriefingDto>> RefreshBriefing([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var briefing = await _briefingService.RefreshBriefingAsync(id, cancellationToken);
            return Ok(briefing);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("organizationId")?.Value ?? User.FindFirst("tenantId")?.Value;
        return Guid.TryParse(claim, out var orgId) ? orgId : Guid.Empty;
    }
}
