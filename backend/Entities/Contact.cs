using backend.Common;

namespace backend.Entities;

public class Contact : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }
    
    // Add additional base properties depending on the module
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
}
