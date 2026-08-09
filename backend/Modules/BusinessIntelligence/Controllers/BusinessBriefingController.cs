using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.BusinessIntelligence.Controllers;

[ApiController]
[Route("api/business-intelligence/briefing")]
[Authorize] // Assuming standard auth is in place
public class BusinessBriefingController : ControllerBase
{
    private readonly IBusinessBriefingService _briefingService;

    public BusinessBriefingController(IBusinessBriefingService briefingService)
    {
        _briefingService = briefingService;
    }

    [HttpGet("today")]
    public async Task<IActionResult> GetTodayBriefing(CancellationToken cancellationToken)
    {
        // Get organizationId from claims. Placeholder logic.
        var organizationId = GetOrganizationId();
        
        var briefing = await _briefingService.GetTodayBriefingAsync(organizationId, cancellationToken);
        
        if (briefing == null)
        {
            // If not found, generate on the fly
            var generated = await _briefingService.GenerateBriefingAsync(organizationId, DateTime.UtcNow.Date, forceRegeneration: false, cancellationToken);
            return Ok(generated);
        }
        
        return Ok(briefing);
    }

    [HttpGet("{date}")]
    public async Task<IActionResult> GetBriefingByDate(DateTime date, CancellationToken cancellationToken)
    {
        var organizationId = GetOrganizationId();
        var briefing = await _briefingService.GetBriefingByDateAsync(organizationId, date, cancellationToken);
        
        if (briefing == null)
            return NotFound(new { message = "No briefing found for the specified date." });
            
        return Ok(briefing);
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateBriefing([FromBody] GenerateBriefingRequest request, CancellationToken cancellationToken)
    {
        var organizationId = GetOrganizationId();
        var targetDate = request.Date?.Date ?? DateTime.UtcNow.Date;
        
        var briefing = await _briefingService.GenerateBriefingAsync(organizationId, targetDate, request.ForceRegeneration, cancellationToken);
        return Ok(briefing);
    }
    
    private Guid GetOrganizationId()
    {
        // Ideally this comes from User.GetOrganizationId() extension method.
        // Hardcoded for test purposes if claim is not present.
        var claim = User.FindFirst("OrganizationId")?.Value;
        return claim != null ? Guid.Parse(claim) : Guid.Parse("00000000-0000-0000-0000-000000000001");
    }
}
