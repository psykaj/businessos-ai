using backend.Common;
using backend.Entities;

namespace backend.Modules.EventBus.Entities;

public class EventLog : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public string EventType { get; set; } = string.Empty;
    
    public string Payload { get; set; } = string.Empty; // JSON representation of the event
    
    public string Source { get; set; } = string.Empty; // Where the event originated from
}
