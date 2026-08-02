using System;
using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Modules.CommunicationHub.Entities;

public class ConversationStatusEntity : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty; // e.g. New, Open, Pending, Resolved, Closed

    [MaxLength(20)]
    public string ColorCode { get; set; } = "#3B82F6";

    public bool IsDefault { get; set; } = false;

    public int SlaThresholdMinutes { get; set; } = 120; // Default SLA response threshold in minutes

    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;
}
