namespace backend.Modules.Analytics.DTOs;

public class DeviceAnalyticsDto
{
    public string DeviceType { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percentage { get; set; }
}
