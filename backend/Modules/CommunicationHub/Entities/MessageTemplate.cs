using System;
using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Modules.CommunicationHub.Entities;

public class MessageTemplate : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public CommunicationChannelType? ChannelType { get; set; } // null means omnichannel

    [MaxLength(50)]
    public string Category { get; set; } = "General"; // Sales, Support, Billing, General

    [MaxLength(50)]
    public string ShortcutCode { get; set; } = string.Empty; // e.g. /welcome, /refund

    public string ParametersJson { get; set; } = "[]"; // List of variable names like customer_name, company_name

    public int UsageCount { get; set; } = 0;
}
