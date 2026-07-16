using backend.Common;

namespace backend.Entities;

public class QRCode : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }
    
    // Add additional base properties depending on the module
    public string Name { get; set; } = string.Empty;
}
