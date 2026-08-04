using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.BusinessPerformance.Entities;

namespace backend.Modules.BusinessPerformance.Interfaces;

public interface IBusinessPerformanceRepository
{
    Task<BusinessMetric?> GetByIdAsync(Guid id, Guid organizationId);
    Task<PagedResult<BusinessMetric>> GetMetricsAsync(Guid organizationId, string? metricType, string? category, int pageNumber, int pageSize);
    Task<List<BusinessMetric>> GetLatestMetricsByPeriodAsync(Guid organizationId, string period);
    Task<BusinessMetric> CreateAsync(BusinessMetric metric);
    Task<BusinessMetric> UpdateAsync(BusinessMetric metric);
    Task<bool> DeleteAsync(Guid id, Guid organizationId);
    Task<List<BusinessMetric>> GetAllActiveByOrgAsync(Guid organizationId);
}
