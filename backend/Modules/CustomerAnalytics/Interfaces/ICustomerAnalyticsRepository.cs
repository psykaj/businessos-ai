using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.CustomerAnalytics.Entities;

namespace backend.Modules.CustomerAnalytics.Interfaces;

public interface ICustomerAnalyticsRepository
{
    Task<CustomerPerformance?> GetByIdAsync(Guid id, Guid organizationId);
    Task<PagedResult<CustomerPerformance>> GetPerformancesAsync(Guid organizationId, string? segment, string? status, int pageNumber, int pageSize);
    Task<List<CustomerPerformance>> GetAllActiveAsync(Guid organizationId);
    Task<CustomerPerformance> CreateAsync(CustomerPerformance performance);
    Task<bool> DeleteAsync(Guid id, Guid organizationId);
}
