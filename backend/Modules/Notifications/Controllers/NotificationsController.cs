using backend.Entities;
using backend.Modules.Notifications.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Modules.Notifications.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationRepository notificationRepository, INotificationService notificationService)
    {
        _notificationRepository = notificationRepository;
        _notificationService = notificationService;
    }

    private Guid GetOrganizationId()
    {
        var orgClaim = User.FindFirst("OrganizationId")?.Value;
        return Guid.TryParse(orgClaim, out var orgId) ? orgId : Guid.Empty;
    }

    private string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications([FromQuery] int limit = 50)
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();
        var notifications = await _notificationRepository.GetUserNotificationsAsync(orgId, userId, limit);
        return Ok(notifications);
    }

    [HttpGet("unread")]
    public async Task<IActionResult> GetUnreadNotifications()
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();
        var notifications = await _notificationRepository.GetUnreadNotificationsAsync(orgId, userId);
        return Ok(notifications);
    }

    [HttpPost("mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();
        await _notificationRepository.MarkAllAsReadAsync(orgId, userId);
        return Ok();
    }
}
