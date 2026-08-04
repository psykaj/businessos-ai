using System;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.ProductAnalytics.DTOs;

namespace backend.Modules.ProductAnalytics.Interfaces;

public interface IProductAnalyticsService
{
    Task<ProductPerformanceSummaryDto> GetSummaryAsync(Guid organizationId);
    Task<PagedResult<ProductPerformanceDto>> GetPerformancesAsync(Guid organizationId, string? period, string? category, bool? topPerformers, int pageNumber, int pageSize);
    Task<ProductPerformanceDto> RecordPerformanceAsync(Guid organizationId, CreateProductPerformanceRequest request, string? userId = null);
    Task<bool> DeletePerformanceAsync(Guid id, Guid organizationId);
    Task<string> ExportProductReportAsync(Guid organizationId);
}
