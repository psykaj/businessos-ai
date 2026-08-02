using System;
using System.Collections.Generic;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Channels.DTOs;

public class OutboundMessageRequest
{
    public Guid ConversationId { get; set; }
    public string RecipientIdentifier { get; set; } = string.Empty; // Phone number, email, page id
    public string Content { get; set; } = string.Empty;
    public string AttachmentsJson { get; set; } = "[]";
    public string ChannelConfigJson { get; set; } = "{}";
    public CommunicationChannelType ChannelType { get; set; }
}

public class OutboundMessageResult
{
    public bool Success { get; set; }
    public string? ExternalMessageId { get; set; }
    public string? ErrorMessage { get; set; }
    public DeliveryStatus Status { get; set; } = DeliveryStatus.Sent;
}

public class InboundMessageResult
{
    public string SenderIdentifier { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string AttachmentsJson { get; set; } = "[]";
    public string? ExternalMessageId { get; set; }
    public CommunicationChannelType ChannelType { get; set; }
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
}

public class ChannelDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public CommunicationChannelType ChannelType { get; set; }
    public string? ProviderIdentifier { get; set; }
    public string WebhookUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
    public int TotalInboundCount { get; set; }
    public int TotalOutboundCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateChannelRequest
{
    public string Name { get; set; } = string.Empty;
    public CommunicationChannelType ChannelType { get; set; }
    public string? ProviderIdentifier { get; set; }
    public string ConfigurationJson { get; set; } = "{}";
    public bool IsDefault { get; set; }
}

public class UpdateChannelRequest
{
    public string Name { get; set; } = string.Empty;
    public string? ProviderIdentifier { get; set; }
    public string ConfigurationJson { get; set; } = "{}";
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
}
