namespace backend.Modules.Analytics.DTOs;

public class BrowserAnalyticsDto
{
    public string Browser { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percentage { get; set; }
}
