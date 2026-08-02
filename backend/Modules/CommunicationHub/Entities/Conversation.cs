using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.CommunicationHub.Entities;

public enum ConversationPriority
{
    Low,
    Medium,
    High,
    Urgent
}

public class Conversation : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    public Guid? ChannelId { get; set; }

    [Required]
    public CommunicationChannelType ChannelType { get; set; }

    [Required]
    [MaxLength(200)]
    public string Subject { get; set; } = string.Empty;

    public Guid? CustomerId { get; set; }

    [MaxLength(150)]
    public string CustomerName { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? CustomerEmail { get; set; }

    [MaxLength(50)]
    public string? CustomerPhone { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "New"; // References ConversationStatusEntity Name or Enum

    public ConversationPriority Priority { get; set; } = ConversationPriority.Medium;

    public Guid? AssignedToUserId { get; set; }

    [MaxLength(150)]
    public string? AssignedToUserName { get; set; }

    public int UnreadMessagesCount { get; set; } = 0;

    public DateTime LastMessageAt { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string LastMessagePreview { get; set; } = string.Empty;

    public DateTime? SlaDueDate { get; set; }

    public bool IsSlaBreached { get; set; } = false;

    public DateTime? ResolvedAt { get; set; }

    [Range(1, 5)]
    public int? CsatRating { get; set; } // Customer Satisfaction Rating

    [MaxLength(500)]
    public string? CsatFeedback { get; set; }

    [MaxLength(500)]
    public string Tags { get; set; } = string.Empty; // Comma separated tags for quick filtering & search

    public string MetadataJson { get; set; } = "{}"; // Extra customer details or channel variables
}
