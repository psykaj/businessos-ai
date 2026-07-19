using backend.Entities;

namespace backend.Modules.Notifications.Interfaces;

public interface INotificationRepository
{
    Task<Notification?> GetByIdAsync(Guid organizationId, Guid id);
    Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid organizationId, string userId, int limit = 50);
    Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(Guid organizationId, string userId);
    Task<Notification> AddAsync(Notification notification);
    Task UpdateAsync(Notification notification);
    Task MarkAllAsReadAsync(Guid organizationId, string userId);
}
