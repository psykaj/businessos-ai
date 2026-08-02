using System;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Messages.DTOs;

public class MessageDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid ConversationId { get; set; }
    public MessageDirection Direction { get; set; }
    public string Content { get; set; } = string.Empty;
    public string AttachmentsJson { get; set; } = "[]";
    public string SenderName { get; set; } = string.Empty;
    public Guid? SenderId { get; set; }
    public DeliveryStatus Status { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string? ExternalMessageId { get; set; }
    public string QuickReplyMetadataJson { get; set; } = "{}";
    public DateTime CreatedAt { get; set; }
}

public class SendMessageRequest
{
    public Guid ConversationId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string AttachmentsJson { get; set; } = "[]";
    public string QuickReplyMetadataJson { get; set; } = "{}";
    public string? TemplateShortcutCode { get; set; } // If using a template
}

public class AddInternalNoteRequest
{
    public Guid ConversationId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string AttachmentsJson { get; set; } = "[]";
}

public class MarkMessagesReadRequest
{
    public Guid ConversationId { get; set; }
}
