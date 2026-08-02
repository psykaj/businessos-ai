using System;
using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Modules.CommunicationHub.Entities;

public enum CommunicationNotificationType
{
    NewMessage,
    ConversationAssigned,
    SlaBreachingSoon,
    SlaBreached,
    CustomerFeedbackReceived
}

public class CommunicationNotification : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid TargetUserId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Content { get; set; } = string.Empty;

    public Guid? ConversationId { get; set; }

    public CommunicationChannelType? ChannelType { get; set; }

    public CommunicationNotificationType NotificationType { get; set; } = CommunicationNotificationType.NewMessage;

    public bool IsRead { get; set; } = false;

    public DateTime? ReadAt { get; set; }
}
