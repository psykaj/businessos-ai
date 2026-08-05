using backend.Modules.RegionalReports.DTOs;

namespace backend.Modules.RegionalReports.Interfaces;

public interface IRegionalReportRepository
{
    Task<IEnumerable<BranchRevenueSummaryDto>> GetRevenueByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<BranchProfitSummaryDto>> GetProfitByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<BranchInventorySummaryDto>> GetInventoryByBranchAsync(Guid organizationId, string? region = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<BranchCustomerSummaryDto>> GetCustomerCountByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<BranchEmployeeSummaryDto>> GetEmployeeCountByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<BranchRankingSummaryDto>> GetBranchRankingAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default);
}

public interface IRegionalReportService
{
    Task<IEnumerable<BranchRevenueSummaryDto>> GetRevenueByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<BranchProfitSummaryDto>> GetProfitByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<BranchInventorySummaryDto>> GetInventoryByBranchAsync(Guid organizationId, string? region = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<BranchCustomerSummaryDto>> GetCustomerCountByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<BranchEmployeeSummaryDto>> GetEmployeeCountByBranchAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<BranchRankingSummaryDto>> GetBranchRankingAsync(Guid organizationId, int year, int month, string? region = null, CancellationToken cancellationToken = default);
}
