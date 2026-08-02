using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Entities;
using backend.Modules.CommunicationHub.Notifications.DTOs;

namespace backend.Modules.CommunicationHub.Notifications.Interfaces;

public interface ICommunicationNotificationService
{
    Task<IEnumerable<CommunicationNotificationDto>> GetUserNotificationsAsync(Guid organizationId, Guid targetUserId, bool unreadOnly);
    Task<CommunicationNotificationDto?> MarkAsReadAsync(Guid id, Guid organizationId);
    Task MarkAllAsReadAsync(Guid organizationId, Guid targetUserId);
    Task<CommunicationNotificationDto> SendNotificationAsync(Guid organizationId, Guid targetUserId, string title, string content, Guid? conversationId, CommunicationChannelType? channelType, CommunicationNotificationType type);
}
