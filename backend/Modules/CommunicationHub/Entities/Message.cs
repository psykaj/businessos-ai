using System;
using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Modules.CommunicationHub.Entities;

public enum MessageDirection
{
    Inbound,
    Outbound,
    InternalNote
}

public enum DeliveryStatus
{
    Sending,
    Sent,
    Delivered,
    Read,
    Failed
}

public class Message : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid ConversationId { get; set; }

    [Required]
    public MessageDirection Direction { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    public string AttachmentsJson { get; set; } = "[]"; // Array of URL, filename, file type, file size

    [MaxLength(150)]
    public string SenderName { get; set; } = string.Empty;

    public Guid? SenderId { get; set; }

    public DeliveryStatus Status { get; set; } = DeliveryStatus.Sent;

    public DateTime? ReadAt { get; set; }

    public DateTime? DeliveredAt { get; set; }

    [MaxLength(200)]
    public string? ExternalMessageId { get; set; } // Provider specific message UUID (WhatsApp message ID, Email Message-ID, etc.)

    public string QuickReplyMetadataJson { get; set; } = "{}"; // Button options or interactive elements
}
