namespace backend.Modules.Analytics.DTOs;

public class CountryAnalyticsDto
{
    public string Country { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percentage { get; set; }
}
