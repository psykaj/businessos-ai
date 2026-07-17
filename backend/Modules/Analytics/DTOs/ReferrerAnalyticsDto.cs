namespace backend.Modules.Analytics.DTOs;

public class ReferrerAnalyticsDto
{
    public string Referrer { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percentage { get; set; }
}
