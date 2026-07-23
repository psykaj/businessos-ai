namespace backend.Modules.BusinessIntelligence.DTOs;

public class KPIDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal CurrentValue { get; set; }
    public decimal? TargetValue { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string Trend { get; set; } = string.Empty;
    public DateTime LastCalculatedAt { get; set; }
}

public class RecalculateKPIsResponseDto
{
    public int UpdatedCount { get; set; }
    public List<KPIDto> KPIs { get; set; } = new();
    public DateTime RecalculatedAt { get; set; } = DateTime.UtcNow;
}
