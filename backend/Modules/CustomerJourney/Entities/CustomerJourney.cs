using backend.Common;
using backend.Entities;
using backend.Modules.CRM.Entities;

namespace backend.Modules.CustomerJourney.Entities;

public class CustomerJourney : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public Guid LeadId { get; set; }
    public Lead? Lead { get; set; }

    public string CurrentStage { get; set; } = "Visitor"; // Visitor, Lead, Qualified Lead, Customer, Repeat Customer, Loyal Customer
    public string PreviousStage { get; set; } = string.Empty;
    
    public DateTime EnteredAt { get; set; } = DateTime.UtcNow;
}
