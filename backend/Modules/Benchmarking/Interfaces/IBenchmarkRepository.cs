using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.Benchmarking.Entities;

namespace backend.Modules.Benchmarking.Interfaces;

public interface IBenchmarkRepository
{
    Task<BenchmarkMetric?> GetByIdAsync(Guid id, Guid organizationId);
    Task<PagedResult<BenchmarkMetric>> GetMetricsAsync(Guid organizationId, string? comparisonType, int pageNumber, int pageSize);
    Task<List<BenchmarkMetric>> GetAllActiveAsync(Guid organizationId);
    Task<BenchmarkMetric> CreateAsync(BenchmarkMetric metric);
    Task<bool> DeleteAsync(Guid id, Guid organizationId);
}
