using System;
using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Modules.CommunicationHub.Entities;

public enum AssignmentStatus
{
    Active,
    Reassigned,
    Completed
}

public class Assignment : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid ConversationId { get; set; }

    [Required]
    public Guid AssignedToUserId { get; set; }

    [MaxLength(150)]
    public string AssignedToUserName { get; set; } = string.Empty;

    public Guid? AssignedByUserId { get; set; }

    [MaxLength(150)]
    public string? AssignedByUserName { get; set; }

    [MaxLength(500)]
    public string? AssignmentReason { get; set; }

    public AssignmentStatus Status { get; set; } = AssignmentStatus.Active;

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
