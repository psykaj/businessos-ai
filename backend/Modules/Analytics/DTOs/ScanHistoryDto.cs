using backend.Common;

namespace backend.Modules.Analytics.DTOs;

public class ScanHistoryDto
{
    public Guid Id { get; set; }
    public Guid QRCodeId { get; set; }
    public string QRCodeName { get; set; } = string.Empty;
    public DateTime ScanDateTime { get; set; }
    public string? IPAddress { get; set; }
    public string? DeviceType { get; set; }
    public string? Browser { get; set; }
    public string? OperatingSystem { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Referrer { get; set; }
    public string? UTMCampaign { get; set; }
}

public class ScanHistoryResponseDto
{
    public PagedResult<ScanHistoryDto> Data { get; set; } = null!;
}
