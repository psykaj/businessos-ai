using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.Benchmarks.Entities;

namespace backend.Modules.Benchmarks.Repositories;

public interface IBenchmarkRepository
{
    Task<IEnumerable<Benchmark>> GetAllAsync(Guid organizationId);
    Task<Benchmark?> GetByMetricAsync(string metricName, Guid organizationId);
    Task<Benchmark> AddAsync(Benchmark benchmark);
}
