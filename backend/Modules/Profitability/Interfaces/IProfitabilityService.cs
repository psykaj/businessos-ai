using System;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.Profitability.DTOs;

namespace backend.Modules.Profitability.Interfaces;

public interface IProfitabilityService
{
    Task<ProfitabilityAnalysisDto> GetAnalysisAsync(Guid organizationId);
    Task<PagedResult<ProfitSnapshotDto>> GetSnapshotsAsync(Guid organizationId, string? period, int pageNumber, int pageSize);
    Task<ProfitSnapshotDto> RecordSnapshotAsync(Guid organizationId, CreateProfitSnapshotRequest request, string? userId = null);
    Task<bool> DeleteSnapshotAsync(Guid id, Guid organizationId);
    Task<string> ExportProfitReportAsync(Guid organizationId);
}
