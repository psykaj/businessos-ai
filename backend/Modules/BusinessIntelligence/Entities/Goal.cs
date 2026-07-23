using backend.Common;

namespace backend.Modules.BusinessIntelligence.Entities;

public class Goal : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal TargetValue { get; set; }
    public decimal CurrentValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = "NotStarted"; // "NotStarted", "InProgress", "Achieved", "Behind", "Failed"
}
