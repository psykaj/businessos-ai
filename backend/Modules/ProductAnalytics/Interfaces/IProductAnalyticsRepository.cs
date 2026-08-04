using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.ProductAnalytics.Entities;

namespace backend.Modules.ProductAnalytics.Interfaces;

public interface IProductAnalyticsRepository
{
    Task<ProductPerformance?> GetByIdAsync(Guid id, Guid organizationId);
    Task<PagedResult<ProductPerformance>> GetPerformancesAsync(Guid organizationId, string? period, string? category, bool? topPerformers, int pageNumber, int pageSize);
    Task<List<ProductPerformance>> GetTopPerformersAsync(Guid organizationId, int limit = 5);
    Task<List<ProductPerformance>> GetLeastPerformersAsync(Guid organizationId, int limit = 5);
    Task<ProductPerformance> CreateAsync(ProductPerformance performance);
    Task<bool> DeleteAsync(Guid id, Guid organizationId);
    Task<List<ProductPerformance>> GetAllActiveAsync(Guid organizationId);
}
