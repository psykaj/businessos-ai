using backend.Common;
using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.Entities;

public class Integration : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public IntegrationProvider Provider { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? ApiKey { get; set; }        // Encrypted AES-256
    public string? AccessToken { get; set; }   // Encrypted AES-256
    public string? RefreshToken { get; set; }  // Encrypted AES-256
    public IntegrationStatus Status { get; set; } = IntegrationStatus.Active;
    public string MetadataJson { get; set; } = "{}";
}
