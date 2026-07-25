using backend.Common;
using backend.Entities;

namespace backend.Modules.CustomerSuccess.SuccessTasks.Entities;

public class SuccessTask : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }


    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public Guid? AssignedUserId { get; set; }
    public User? AssignedUser { get; set; }

    public string TaskType { get; set; } = "Manual"; // FollowUpInactive, ContactDissatisfied, WelcomeNew, CongratulateMilestone, UpsellHighValue, Manual
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }
    public string Priority { get; set; } = "Medium"; // Low, Medium, High, Critical
    public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, Cancelled
}
