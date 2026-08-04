using System;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.CustomerAnalytics.DTOs;

namespace backend.Modules.CustomerAnalytics.Interfaces;

public interface ICustomerAnalyticsService
{
    Task<CustomerAnalyticsSummaryDto> GetSummaryAsync(Guid organizationId);
    Task<PagedResult<CustomerPerformanceDto>> GetPerformancesAsync(Guid organizationId, string? segment, string? status, int pageNumber, int pageSize);
    Task<CustomerPerformanceDto> RecordPerformanceAsync(Guid organizationId, CreateCustomerPerformanceRequest request, string? userId = null);
    Task<bool> DeletePerformanceAsync(Guid id, Guid organizationId);
    Task<string> ExportCustomerReportAsync(Guid organizationId);
}
