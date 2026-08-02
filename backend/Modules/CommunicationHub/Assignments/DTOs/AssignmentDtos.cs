using System;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Assignments.DTOs;

public class AssignmentDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid ConversationId { get; set; }
    public Guid AssignedToUserId { get; set; }
    public string AssignedToUserName { get; set; } = string.Empty;
    public Guid? AssignedByUserId { get; set; }
    public string? AssignedByUserName { get; set; }
    public string? AssignmentReason { get; set; }
    public AssignmentStatus Status { get; set; }
    public DateTime AssignedAt { get; set; }
}

public class AssignmentFilterDto
{
    public Guid? ConversationId { get; set; }
    public Guid? AssignedToUserId { get; set; }
}
