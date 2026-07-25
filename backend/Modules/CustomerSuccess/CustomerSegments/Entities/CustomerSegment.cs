using backend.Common;
using backend.Entities;

namespace backend.Modules.CustomerSuccess.CustomerSegments.Entities;

public class CustomerSegment : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }


    public string Name { get; set; } = string.Empty; // New Customers, Active Customers, VIP Customers, High Spend Customers, Repeat Customers, Inactive Customers, At-Risk Customers
    public string SegmentType { get; set; } = "Automated"; // Automated, Custom
    public string? CriteriaJson { get; set; }
    public int CustomerCount { get; set; } = 0;
}
