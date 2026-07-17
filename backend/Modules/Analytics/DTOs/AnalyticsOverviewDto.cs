namespace backend.Modules.Analytics.DTOs;

public class AnalyticsOverviewDto
{
    public int TotalScans { get; set; }
    public int UniqueScans { get; set; }
    public int ScansToday { get; set; }
    public int ActiveQRCodes { get; set; }
}
