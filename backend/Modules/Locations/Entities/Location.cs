using backend.Common;

namespace backend.Modules.Locations.Entities;

public class Location : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty; // e.g., "North America", "EMEA", "APAC", "US-East"
    public string Country { get; set; } = string.Empty;
    public string? State { get; set; }
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? PostalCode { get; set; }
    
    public string TimeZone { get; set; } = "UTC";
    public string Currency { get; set; } = "USD";
    
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    
    public bool IsActive { get; set; } = true;
}
