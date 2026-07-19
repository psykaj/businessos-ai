using backend.Entities;
using backend.Modules.Notifications.Interfaces;
using backend.Modules.Notifications.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace backend.Modules.Notifications.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        INotificationRepository notificationRepository, 
        IHubContext<NotificationHub> hubContext,
        ILogger<NotificationService> logger)
    {
        _notificationRepository = notificationRepository;
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task SendNotificationAsync(Guid organizationId, string userId, string title, string message, string type = "Info")
    {
        var notification = new Notification
        {
            OrganizationId = organizationId,
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            Status = "Unread"
        };

        await _notificationRepository.AddAsync(notification);
        
        await _hubContext.Clients.Group($"User_{userId}").SendAsync("ReceiveNotification", notification);
        _logger.LogInformation($"Notification sent to user {userId}: {title}");
    }

    public async Task BroadcastToOrganizationAsync(Guid organizationId, string title, string message, string type = "Info")
    {
        var notificationMsg = new 
        {
            Title = title,
            Message = message,
            Type = type,
            CreatedAt = DateTime.UtcNow
        };
        
        await _hubContext.Clients.Group($"Org_{organizationId}").SendAsync("ReceiveBroadcast", notificationMsg);
        _logger.LogInformation($"Broadcast to org {organizationId}: {title}");
    }
}
