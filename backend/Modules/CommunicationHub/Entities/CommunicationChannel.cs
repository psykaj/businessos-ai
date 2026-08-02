using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.CommunicationHub.Entities;

public enum CommunicationChannelType
{
    Email,
    WhatsApp,
    SMS,
    LiveChat,
    FacebookMessenger,
    InstagramDm
}

public class CommunicationChannel : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public CommunicationChannelType ChannelType { get; set; }

    [MaxLength(255)]
    public string? ProviderIdentifier { get; set; } // e.g. Email address, Phone number, Page ID

    public string ConfigurationJson { get; set; } = "{}"; // Provider credentials & adapter tokens

    [MaxLength(500)]
    public string WebhookUrl { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public bool IsDefault { get; set; } = false;

    public int TotalInboundCount { get; set; } = 0;

    public int TotalOutboundCount { get; set; } = 0;
}
