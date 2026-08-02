using System;
using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Modules.CommunicationHub.Entities;

public class ConversationTag : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string ColorCode { get; set; } = "#10B981";

    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    public int UsageCount { get; set; } = 0;
}
