using System;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.RevenueAnalytics.DTOs;

namespace backend.Modules.RevenueAnalytics.Interfaces;

public interface IRevenueAnalyticsService
{
    Task<RevenueAnalyticsSummaryDto> GetRevenueSummaryAsync(Guid organizationId);
    Task<PagedResult<RevenueSnapshotDto>> GetSnapshotsAsync(Guid organizationId, string? periodType, int pageNumber, int pageSize);
    Task<RevenueSnapshotDto> RecordSnapshotAsync(Guid organizationId, CreateRevenueSnapshotRequest request, string? userId = null);
    Task<bool> DeleteSnapshotAsync(Guid id, Guid organizationId);
    Task<string> ExportRevenueReportAsync(Guid organizationId);
}
