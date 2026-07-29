using backend.Common;
using backend.Entities;

namespace backend.Modules.ApiPlatform.Entities;

public class ApiLog : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public Guid? ApiKeyId { get; set; }
    public ApiKey? ApiKey { get; set; }

    public string RequestPath { get; set; } = string.Empty;
    public string RequestMethod { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    
    public long DurationMilliseconds { get; set; }
    
    // Optional request/response tracking for auditing
    public string? RequestPayload { get; set; }
    public string? ResponsePayload { get; set; }
}
