using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Notifications.Interfaces;

public interface ICommunicationNotificationRepository
{
    Task<IEnumerable<CommunicationNotification>> GetByUserIdAsync(Guid organizationId, Guid targetUserId, bool unreadOnly);
    Task<CommunicationNotification?> GetByIdAsync(Guid id, Guid organizationId);
    Task<CommunicationNotification> CreateAsync(CommunicationNotification notification);
    Task UpdateAsync(CommunicationNotification notification);
    Task MarkAllAsReadAsync(Guid organizationId, Guid targetUserId);
}
