using System;
using System.Collections.Generic;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Inbox.DTOs;

public class ChannelBadgeCountDto
{
    public CommunicationChannelType ChannelType { get; set; }
    public int UnreadCount { get; set; }
    public int ActiveCount { get; set; }
}

public class InboxSummaryDto
{
    public Guid OrganizationId { get; set; }
    public int TotalActiveConversations { get; set; }
    public int TotalUnreadConversations { get; set; }
    public int TotalSlaBreached { get; set; }
    public int TotalUrgentConversations { get; set; }
    public List<ChannelBadgeCountDto> ChannelCounts { get; set; } = new();
    public DateTime LastRefreshedAt { get; set; } = DateTime.UtcNow;
}

public class WebhookIngestResponse
{
    public bool ProcessedSuccessfully { get; set; }
    public Guid? ConversationId { get; set; }
    public Guid? MessageId { get; set; }
    public string? Error { get; set; }
}
