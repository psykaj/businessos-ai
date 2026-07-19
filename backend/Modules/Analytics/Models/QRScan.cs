using backend.Common;
using backend.Entities;
using backend.Modules.QRCode.Models;

namespace backend.Modules.Analytics.Models;

public class QRScan : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Entities.Organization? Organization { get; set; }

    public Guid QRCodeId { get; set; }
    public QRCode.Models.QRCode? QRCode { get; set; }

    public DateTime ScanDateTime { get; set; } = DateTime.UtcNow;
    
    public string? IPAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? DeviceType { get; set; }
    public string? Browser { get; set; }
    public string? BrowserVersion { get; set; }
    public string? OperatingSystem { get; set; }
    public string? OperatingSystemVersion { get; set; }
    
    public string? Country { get; set; }
    public string? State { get; set; }
    public string? City { get; set; }
    public string? TimeZone { get; set; }
    
    public string? Referrer { get; set; }
    public string? UTMSource { get; set; }
    public string? UTMMedium { get; set; }
    public string? UTMCampaign { get; set; }
    public string? UTMTerm { get; set; }
    public string? UTMContent { get; set; }
    
    public string? Language { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
