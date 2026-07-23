using backend.Modules.BusinessIntelligence.DTOs;

namespace backend.Modules.BusinessIntelligence.Interfaces;

public interface IReportGeneratorService
{
    Task<ReportDto> GenerateReportAsync(Guid organizationId, string userId, GenerateReportRequestDto request, CancellationToken cancellationToken = default);
    Task<ReportDto?> GetReportByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<ReportDto>> GetReportsAsync(Guid organizationId, string? reportType = null, CancellationToken cancellationToken = default);
}
