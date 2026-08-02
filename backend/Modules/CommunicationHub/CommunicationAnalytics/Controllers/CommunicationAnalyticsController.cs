using System;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.CommunicationAnalytics.DTOs;
using backend.Modules.CommunicationHub.CommunicationAnalytics.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CommunicationHub.CommunicationAnalytics.Controllers;

[ApiController]
[Route("api/v1/communication-hub/analytics")]
[Authorize]
public class CommunicationAnalyticsController : ControllerBase
{
    private readonly ICommunicationAnalyticsService _service;

    public CommunicationAnalyticsController(ICommunicationAnalyticsService service)
    {
        _service = service;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value ?? User.FindFirst("organizationId")?.Value;
        return claim != null && Guid.TryParse(claim, out var orgId) ? orgId : Guid.Empty;
    }

    [HttpGet("overview")]
    [ProducesResponseType(typeof(AnalyticsOverviewDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<AnalyticsOverviewDto>> GetOverview()
    {
        var orgId = GetOrganizationId();
        if (orgId == Guid.Empty) return Unauthorized();

        var overview = await _service.GetOverviewAsync(orgId);
        return Ok(overview);
    }

    [HttpPost("csat/{conversationId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubmitCsat(Guid conversationId, [FromQuery] Guid organizationId, [FromBody] SubmitCsatRequest request)
    {
        var result = await _service.SubmitCsatRatingAsync(conversationId, organizationId, request);
        if (!result) return NotFound(new { Message = "Conversation not found or ineligible for rating." });
        return Ok(new { Success = true, Message = "Thank you for your feedback!" });
    }
}
