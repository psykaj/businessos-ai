using System;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.MarketingROI.DTOs;

namespace backend.Modules.MarketingROI.Interfaces;

public interface IMarketingRoiService
{
    Task<MarketingRoiSummaryDto> GetSummaryAsync(Guid organizationId);
    Task<PagedResult<MarketingPerformanceDto>> GetPerformancesAsync(Guid organizationId, string? channel, string? period, int pageNumber, int pageSize);
    Task<MarketingPerformanceDto> RecordPerformanceAsync(Guid organizationId, CreateMarketingPerformanceRequest request, string? userId = null);
    Task<bool> DeletePerformanceAsync(Guid id, Guid organizationId);
    Task<string> ExportMarketingReportAsync(Guid organizationId);
}
