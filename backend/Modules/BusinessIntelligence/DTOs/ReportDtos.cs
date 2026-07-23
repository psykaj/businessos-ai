namespace backend.Modules.BusinessIntelligence.DTOs;

public class ReportDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ReportType { get; set; } = string.Empty;
    public string Filters { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
    public string GeneratedBy { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public string? FileUrl { get; set; }
    public object? DataPayload { get; set; }
}

public class GenerateReportRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string ReportType { get; set; } = string.Empty; // "Executive", "Sales", "Marketing", "CRM", "Workflow", "AIUsage"
    public string Format { get; set; } = "JSON"; // "PDF", "Excel", "CSV", "JSON"
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Dictionary<string, string>? FilterParams { get; set; }
}

public class ReportExportResponseDto
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[] FileBytes { get; set; } = Array.Empty<byte>();
}
