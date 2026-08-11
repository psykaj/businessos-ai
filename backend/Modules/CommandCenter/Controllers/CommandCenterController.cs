using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Modules.CommandCenter.DTOs;
using backend.Modules.CommandCenter.Interfaces;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.ActionCenter.DTOs;

namespace backend.Modules.CommandCenter.Controllers;

[ApiController]
[Route("api/command-center")]
[Authorize]
public class CommandCenterController : ControllerBase
{
    private readonly ICommandCenterService _commandCenterService;

    public CommandCenterController(ICommandCenterService commandCenterService)
    {
        _commandCenterService = commandCenterService;
    }

    private Guid GetOrganizationId()
    {
        var orgIdClaim = User.FindFirst("OrganizationId")?.Value ?? User.FindFirst("organizationId")?.Value;
        if (Guid.TryParse(orgIdClaim, out var orgId)) return orgId;
        throw new UnauthorizedAccessException("Organization ID is missing or invalid in the token.");
    }

    private string GetUserId()
    {
        return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
            ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value
            ?? throw new UnauthorizedAccessException("User ID is missing in the token.");
    }

    [HttpGet]
    public async Task<ActionResult<CommandCenterSummaryDto>> GetCommandCenterSummary(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();
        var summary = await _commandCenterService.GetCommandCenterSummaryAsync(orgId, userId, cancellationToken);
        return Ok(summary);
    }

    [HttpGet("metrics")]
    public async Task<ActionResult<IEnumerable<CommandMetricDto>>> GetMetrics(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var metrics = await _commandCenterService.GetMetricsAsync(orgId, cancellationToken);
        return Ok(metrics);
    }

    [HttpGet("alerts")]
    public async Task<ActionResult<IEnumerable<ProactiveAlertDto>>> GetAlerts(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var alerts = await _commandCenterService.GetAlertsAsync(orgId, cancellationToken);
        return Ok(alerts);
    }

    [HttpGet("opportunities")]
    public async Task<ActionResult<IEnumerable<CommandOpportunityDto>>> GetOpportunities(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var opportunities = await _commandCenterService.GetOpportunitiesAsync(orgId, cancellationToken);
        return Ok(opportunities);
    }

    [HttpGet("actions")]
    public async Task<ActionResult<IEnumerable<AiActionDto>>> GetActions(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var actions = await _commandCenterService.GetActionsAsync(orgId, cancellationToken);
        return Ok(actions);
    }

    [HttpGet("automations")]
    public async Task<ActionResult<CommandAutomationSummaryDto>> GetAutomations(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var automations = await _commandCenterService.GetAutomationsAsync(orgId, cancellationToken);
        return Ok(automations);
    }

    [HttpGet("activity")]
    public async Task<ActionResult<IEnumerable<CommandActivityDto>>> GetActivity(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var activities = await _commandCenterService.GetActivityAsync(orgId, cancellationToken);
        return Ok(activities);
    }

    [HttpGet("trends")]
    public async Task<ActionResult<IEnumerable<CommandTrendDto>>> GetTrends(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var trends = await _commandCenterService.GetTrendsAsync(orgId, cancellationToken);
        return Ok(trends);
    }

    [HttpPost("ask")]
    public async Task<ActionResult<CommandCenterAskResponseDto>> AskBusinessOsAi([FromBody] CommandCenterAskRequestDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return BadRequest("Question cannot be empty.");
        }

        var orgId = GetOrganizationId();
        var userId = GetUserId();
        
        var response = await _commandCenterService.AskBusinessOsAiAsync(orgId, userId, request, cancellationToken);
        return Ok(response);
    }
}
