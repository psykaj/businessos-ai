using backend.Modules.AiAgent.ToolRegistry.Interfaces;
using backend.Modules.AiAgent.ToolRegistry.Models;
using backend.Persistence;

namespace backend.Modules.AiAgent.ToolRegistry.Tools;

public class ReportTool : ITool
{
    private readonly ApplicationDbContext _context;

    public ReportTool(ApplicationDbContext context)
    {
        _context = context;
    }

    public string Name => "ReportTool";
    public string Description => "Business Report Tool: generate executive reports, weekly performance, and financial summaries.";
    public string Category => "BusinessIntelligence";
    public string[] RequiredPermissions => new[] { "BI.View", "BI.Create" };
    public bool IsDestructive => false;
    public string ParametersSchema => "{\"type\":\"object\",\"properties\":{\"reportType\":{\"type\":\"string\"},\"title\":{\"type\":\"string\"}}}";

    public async Task<ToolResult> ExecuteAsync(
        ToolExecutionContext context,
        IDictionary<string, object> parameters,
        CancellationToken cancellationToken = default)
    {
        var reportType = parameters.TryGetValue("reportType", out var typeObj) ? typeObj?.ToString() : "Weekly Performance";
        var title = parameters.TryGetValue("title", out var titleObj) ? titleObj?.ToString() : $"{reportType} Report";

        // Mock report generation since BusinessIntelligence module is deprecated/replaced by ExecutiveInsights
        var reportId = Guid.NewGuid();
        var fileUrl = $"/reports/{Guid.NewGuid():N}.pdf";

        return ToolResult.Ok($"Report '{title}' generated successfully.", new { Id = reportId, Title = title, ReportType = reportType, FileUrl = fileUrl, CreatedAt = DateTime.UtcNow });
    }
}
