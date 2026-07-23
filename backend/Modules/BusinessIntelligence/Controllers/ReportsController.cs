using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.BusinessIntelligence.Controllers;

[Route("api/v1/bi/reports")]
public class ReportsController : BaseBIController
{
    private readonly IReportGeneratorService _reportGeneratorService;
    private readonly IReportExportService _reportExportService;

    public ReportsController(
        IReportGeneratorService reportGeneratorService,
        IReportExportService reportExportService)
    {
        _reportGeneratorService = reportGeneratorService;
        _reportExportService = reportExportService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReportDto>>> GetReports([FromQuery] string? reportType, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var reports = await _reportGeneratorService.GetReportsAsync(orgId, reportType, cancellationToken);
        return Ok(reports);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReportDto>> GetReportById(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var report = await _reportGeneratorService.GetReportByIdAsync(id, orgId, cancellationToken);
        if (report == null) return NotFound(new { message = $"Report with id '{id}' not found" });
        return Ok(report);
    }

    [HttpPost("generate")]
    public async Task<ActionResult<ReportDto>> GenerateReport([FromBody] GenerateReportRequestDto request, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId();
        var report = await _reportGeneratorService.GenerateReportAsync(orgId, userId, request, cancellationToken);
        return CreatedAtAction(nameof(GetReportById), new { id = report.Id }, report);
    }

    [HttpGet("{id:guid}/export")]
    public async Task<IActionResult> ExportReport(Guid id, [FromQuery] string format = "csv", CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var report = await _reportGeneratorService.GetReportByIdAsync(id, orgId, cancellationToken);
        if (report == null) return NotFound(new { message = $"Report with id '{id}' not found" });

        var exportData = await _reportExportService.ExportReportAsync(report, format, cancellationToken);
        return File(exportData.FileBytes, exportData.ContentType, exportData.FileName);
    }
}
