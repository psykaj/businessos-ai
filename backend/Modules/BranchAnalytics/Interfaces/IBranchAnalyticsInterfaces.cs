using backend.Modules.BranchAnalytics.Entities;

namespace backend.Modules.BranchAnalytics.Interfaces;

public interface IBranchPerformanceRepository
{
    Task<IEnumerable<BranchPerformance>> GetByOrgAndPeriodAsync(Guid organizationId, int year, int month, CancellationToken cancellationToken = default);
    Task<BranchPerformance?> GetByBranchAndPeriodAsync(Guid branchId, int year, int month, CancellationToken cancellationToken = default);
    Task<BranchPerformance> AddOrUpdateAsync(BranchPerformance performance, CancellationToken cancellationToken = default);
}

public interface IBranchPerformanceEngineService
{
    Task<DTOs.PerformanceEngineSummaryDto> CalculatePerformanceAsync(Guid organizationId, int year, int month, CancellationToken cancellationToken = default);
    Task<DTOs.PerformanceEngineSummaryDto> GetPerformanceSummaryAsync(Guid organizationId, int year, int month, CancellationToken cancellationToken = default);
    Task<IEnumerable<DTOs.MonthlyComparisonResponseDto>> GetMonthlyComparisonsAsync(Guid organizationId, int year, int month, CancellationToken cancellationToken = default);
}
