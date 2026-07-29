using backend.Common;
using backend.Entities;

namespace backend.Modules.OAuth.Entities;

public class OAuthApplication : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string HomepageUrl { get; set; } = string.Empty;
    
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecretHash { get; set; } = string.Empty;
    
    // Comma-separated or JSON array of allowed redirect URIs
    public string RedirectUris { get; set; } = string.Empty;
    
    // Comma-separated list of scopes the app is allowed to request
    public string AllowedScopes { get; set; } = string.Empty;
    
    public string Status { get; set; } = "Active"; // Active, Suspended, Revoked
}
