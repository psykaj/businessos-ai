using System;
using System.Threading.Tasks;
using backend.Common;
using backend.Modules.Benchmarking.DTOs;

namespace backend.Modules.Benchmarking.Interfaces;

public interface IBenchmarkService
{
    Task<BenchmarkComparisonSummaryDto> GetComparisonSummaryAsync(Guid organizationId, string industry = "Enterprise SaaS & AI Software");
    Task<PagedResult<BenchmarkMetricDto>> GetMetricsAsync(Guid organizationId, string? comparisonType, int pageNumber, int pageSize);
    Task<BenchmarkMetricDto> RecordMetricAsync(Guid organizationId, CreateBenchmarkMetricRequest request, string? userId = null);
    Task<bool> DeleteMetricAsync(Guid id, Guid organizationId);
    Task<string> ExportBenchmarkReportAsync(Guid organizationId);
}
