using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.ActionCenter.Entities;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.BusinessIntelligence.Controllers;

[ApiController]
[Route("api/alerts")]
[Authorize]
public class ProactiveAlertController : ControllerBase
{
    private readonly IProactiveAlertService _alertService;
    private readonly ApplicationDbContext _context;

    public ProactiveAlertController(IProactiveAlertService alertService, ApplicationDbContext context)
    {
        _alertService = alertService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAlerts(CancellationToken cancellationToken)
    {
        var organizationId = GetOrganizationId();
        var alerts = await _alertService.GetAlertsAsync(organizationId, cancellationToken);
        return Ok(alerts);
    }

    [HttpGet("unread")]
    public async Task<IActionResult> GetUnreadAlerts(CancellationToken cancellationToken)
    {
        var organizationId = GetOrganizationId();
        var alerts = await _alertService.GetUnreadAlertsAsync(organizationId, cancellationToken);
        return Ok(alerts);
    }

    [HttpGet("action-required")]
    public async Task<IActionResult> GetActionRequiredAlerts(CancellationToken cancellationToken)
    {
        var organizationId = GetOrganizationId();
        var alerts = await _alertService.GetActionRequiredAlertsAsync(organizationId, cancellationToken);
        return Ok(alerts);
    }

    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken cancellationToken)
    {
        var organizationId = GetOrganizationId();
        await _alertService.MarkAsReadAsync(id, organizationId, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id}/dismiss")]
    public async Task<IActionResult> Dismiss(Guid id, CancellationToken cancellationToken)
    {
        var organizationId = GetOrganizationId();
        await _alertService.DismissAsync(id, organizationId, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id}/resolve")]
    public async Task<IActionResult> Resolve(Guid id, CancellationToken cancellationToken)
    {
        var organizationId = GetOrganizationId();
        await _alertService.ResolveAsync(id, organizationId, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id}/action")]
    public async Task<IActionResult> CreateAction(Guid id, [FromBody] BriefingActionRequest request, CancellationToken cancellationToken)
    {
        var organizationId = GetOrganizationId();
        
        var alert = await _context.ProactiveAlerts.FindAsync(new object[] { id }, cancellationToken);
        if (alert == null || alert.OrganizationId != organizationId)
            return NotFound();

        // Create an Action in the Action Center
        var action = new AiAction
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            Title = request.Action,
            Description = $"Action created from alert: {alert.Title}",
            Category = AiActionCategory.Other, // Simplified. Could map based on source module.
            Priority = AiActionPriority.High,
            Status = AiActionStatus.Pending,
            RiskLevel = AiActionRiskLevel.Low,
            CreatedAt = DateTime.UtcNow
        };

        _context.AiActions.Add(action);
        
        // Mark alert as resolved
        alert.Status = Entities.AlertStatus.Resolved;
        alert.ResolvedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(new { message = "Action created successfully in Action Center.", actionId = action.Id });
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value;
        return claim != null ? Guid.Parse(claim) : Guid.Parse("00000000-0000-0000-0000-000000000001");
    }
}
