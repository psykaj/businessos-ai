using backend.Common;
using backend.Entities;

namespace backend.Modules.Connectors.Entities;

public class Connector : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    // E.g., Stripe, Shopify, Slack, GoogleWorkspace
    public string Provider { get; set; } = string.Empty;
    
    public string ConnectionName { get; set; } = string.Empty;
    
    // Encrypted JSON or serialized settings
    public string Settings { get; set; } = string.Empty; 
    
    // Encrypted OAuth tokens or API keys for the third party
    public string Credentials { get; set; } = string.Empty; 
    
    public string Status { get; set; } = "Connected"; // Connected, Disconnected, Error
    
    public DateTime? LastSyncAt { get; set; }
    public string? ErrorMessage { get; set; }
}
