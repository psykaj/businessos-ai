using System;
using System.Collections.Generic;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Conversations.DTOs;

public class ConversationDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid? ChannelId { get; set; }
    public CommunicationChannelType ChannelType { get; set; }
    public string Subject { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public string Status { get; set; } = "New";
    public ConversationPriority Priority { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }
    public int UnreadMessagesCount { get; set; }
    public DateTime LastMessageAt { get; set; }
    public string LastMessagePreview { get; set; } = string.Empty;
    public DateTime? SlaDueDate { get; set; }
    public bool IsSlaBreached { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public int? CsatRating { get; set; }
    public string? CsatFeedback { get; set; }
    public string Tags { get; set; } = string.Empty;
    public string MetadataJson { get; set; } = "{}";
    public DateTime CreatedAt { get; set; }
}

public class CreateConversationRequest
{
    public CommunicationChannelType ChannelType { get; set; }
    public Guid? ChannelId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public ConversationPriority Priority { get; set; } = ConversationPriority.Medium;
    public string InitialMessageContent { get; set; } = string.Empty;
}

public class UpdateConversationStatusRequest
{
    public string Status { get; set; } = string.Empty; // Open, Resolved, Closed, Snoozed
}

public class AssignConversationRequest
{
    public Guid AssignedToUserId { get; set; }
    public string AssignedToUserName { get; set; } = string.Empty;
    public string? AssignmentReason { get; set; }
}

public class AddTagsRequest
{
    public string Tags { get; set; } = string.Empty; // comma-separated tags
}

public class ConversationFilterDto
{
    public CommunicationChannelType? ChannelType { get; set; }
    public string? Status { get; set; }
    public Guid? AssigneeId { get; set; }
    public ConversationPriority? Priority { get; set; }
    public string? SearchKeyword { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
