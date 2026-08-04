using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.BusinessPerformance.DTOs;

namespace backend.Modules.BusinessPerformance.Interfaces;

public interface IBusinessPerformanceService
{
    Task<BusinessPerformanceDashboardDto> GetDashboardAsync(Guid organizationId);
    Task<PagedResult<BusinessMetricDto>> GetMetricsAsync(Guid organizationId, string? metricType, string? category, int pageNumber, int pageSize);
    Task<BusinessMetricDto> CreateMetricAsync(Guid organizationId, CreateBusinessMetricRequest request, string? userId = null);
    Task<BusinessMetricDto> UpdateMetricAsync(Guid id, Guid organizationId, UpdateBusinessMetricRequest request, string? userId = null);
    Task<bool> DeleteMetricAsync(Guid id, Guid organizationId);
    Task<BusinessPerformanceDashboardDto> CalculateAndRefreshEngineAsync(Guid organizationId);
    Task<BusinessPerformanceExportDto> ExportPerformanceReportAsync(Guid organizationId, string format = "CSV");
}
