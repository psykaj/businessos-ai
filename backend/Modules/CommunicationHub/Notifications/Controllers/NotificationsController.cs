using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Notifications.DTOs;
using backend.Modules.CommunicationHub.Notifications.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CommunicationHub.Notifications.Controllers;

[ApiController]
[Route("api/v1/communication-hub/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly ICommunicationNotificationService _service;

    public NotificationsController(ICommunicationNotificationService service)
    {
        _service = service;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value ?? User.FindFirst("organizationId")?.Value;
        return claim != null && Guid.TryParse(claim, out var orgId) ? orgId : Guid.Empty;
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        return claim != null && Guid.TryParse(claim, out var id) ? id : Guid.Empty;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CommunicationNotificationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CommunicationNotificationDto>>> GetMyNotifications([FromQuery] bool unreadOnly = true)
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();
        if (orgId == Guid.Empty || userId == Guid.Empty) return Unauthorized();

        var notifications = await _service.GetUserNotificationsAsync(orgId, userId, unreadOnly);
        return Ok(notifications);
    }

    [HttpPost("{id:guid}/mark-read")]
    [ProducesResponseType(typeof(CommunicationNotificationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CommunicationNotificationDto>> MarkRead(Guid id)
    {
        var orgId = GetOrganizationId();
        var updated = await _service.MarkAsReadAsync(id, orgId);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpPost("mark-all-read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> MarkAllRead()
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();
        await _service.MarkAllAsReadAsync(orgId, userId);
        return Ok(new { Success = true });
    }
}
