using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Helpers;
using backend.Modules.BusinessIntelligence.Interfaces;

namespace backend.Modules.BusinessIntelligence.Services;

public class ReportExportService : IReportExportService
{
    public Task<ReportExportResponseDto> ExportReportAsync(ReportDto report, string format, CancellationToken cancellationToken = default)
    {
        var fmt = string.IsNullOrWhiteSpace(format) ? "csv" : format.ToLower();

        var (content, contentType, ext) = fmt switch
        {
            "pdf" => ExportHelper.ExportToPdf(report),
            "excel" or "xlsx" or "xls" => ExportHelper.ExportToExcel(report),
            _ => ExportHelper.ExportToCsv(report)
        };

        var safeName = report.Name.Replace(" ", "_").Replace("/", "_");
        var fileName = $"{safeName}_{DateTime.UtcNow:yyyyMMddHHmmss}.{ext}";

        return Task.FromResult(new ReportExportResponseDto
        {
            FileName = fileName,
            ContentType = contentType,
            FileBytes = content
        });
    }
}
