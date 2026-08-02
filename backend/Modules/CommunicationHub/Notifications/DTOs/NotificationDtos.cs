using System;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Notifications.DTOs;

public class CommunicationNotificationDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid TargetUserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid? ConversationId { get; set; }
    public CommunicationChannelType? ChannelType { get; set; }
    public CommunicationNotificationType NotificationType { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
