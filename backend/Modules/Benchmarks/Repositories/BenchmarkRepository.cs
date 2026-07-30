using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.Benchmarks.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Benchmarks.Repositories;

public class BenchmarkRepository : IBenchmarkRepository
{
    private readonly ApplicationDbContext _context;

    public BenchmarkRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Benchmark>> GetAllAsync(Guid organizationId)
    {
        return await _context.Benchmarks
            .Where(b => b.OrganizationId == organizationId && !b.IsDeleted)
            .ToListAsync();
    }

    public async Task<Benchmark?> GetByMetricAsync(string metricName, Guid organizationId)
    {
        return await _context.Benchmarks
            .Where(b => b.MetricName == metricName && b.OrganizationId == organizationId && !b.IsDeleted && b.ValidTo >= DateTime.UtcNow)
            .OrderByDescending(b => b.ValidFrom)
            .FirstOrDefaultAsync();
    }

    public async Task<Benchmark> AddAsync(Benchmark benchmark)
    {
        _context.Benchmarks.Add(benchmark);
        await _context.SaveChangesAsync();
        return benchmark;
    }
}
