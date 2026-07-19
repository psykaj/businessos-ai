using backend.Entities;

namespace backend.Modules.Notifications.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(Guid organizationId, string userId, string title, string message, string type = "Info");
    Task BroadcastToOrganizationAsync(Guid organizationId, string title, string message, string type = "Info");
}
