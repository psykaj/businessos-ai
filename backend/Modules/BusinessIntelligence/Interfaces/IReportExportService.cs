using backend.Modules.BusinessIntelligence.DTOs;

namespace backend.Modules.BusinessIntelligence.Interfaces;

public interface IReportExportService
{
    Task<ReportExportResponseDto> ExportReportAsync(ReportDto report, string format, CancellationToken cancellationToken = default);
}
