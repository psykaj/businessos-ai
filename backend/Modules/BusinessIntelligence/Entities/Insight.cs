using backend.Common;

namespace backend.Modules.BusinessIntelligence.Entities;

public class Insight : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public string Category { get; set; } = string.Empty; // "Sales", "Marketing", "CRM", "Finance", "Operations"
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium"; // "Low", "Medium", "High", "Critical"
    public string Recommendation { get; set; } = string.Empty;
    public string BusinessImpact { get; set; } = string.Empty;
    public string SuggestedAction { get; set; } = string.Empty;
}
