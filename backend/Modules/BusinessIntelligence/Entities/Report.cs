using backend.Common;

namespace backend.Modules.BusinessIntelligence.Entities;

public class Report : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ReportType { get; set; } = string.Empty; // "Executive", "Sales", "Marketing", "CRM", "Workflow", "AIUsage"
    public string Filters { get; set; } = "{}"; // JSON formatted filters
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public string GeneratedBy { get; set; } = string.Empty;
    public string Format { get; set; } = "JSON"; // "PDF", "Excel", "CSV", "JSON"
    public string? FileUrl { get; set; }
}
