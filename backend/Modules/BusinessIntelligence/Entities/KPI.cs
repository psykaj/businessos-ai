using backend.Common;

namespace backend.Modules.BusinessIntelligence.Entities;

public class KPI : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal CurrentValue { get; set; }
    public decimal? TargetValue { get; set; }
    public string Unit { get; set; } = string.Empty; // e.g., "$", "%", "count", "days"
    public string Trend { get; set; } = "Neutral"; // e.g., "Up", "Down", "Neutral", "+12.5%"
    public DateTime LastCalculatedAt { get; set; } = DateTime.UtcNow;
}
