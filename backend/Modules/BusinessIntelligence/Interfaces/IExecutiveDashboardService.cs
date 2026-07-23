using backend.Modules.BusinessIntelligence.DTOs;

namespace backend.Modules.BusinessIntelligence.Interfaces;

public interface IExecutiveDashboardService
{
    Task<CEODashboardDto> GetCEODashboardAsync(Guid organizationId, DashboardFilterDto filter, CancellationToken cancellationToken = default);
    Task<SalesDashboardDto> GetSalesDashboardAsync(Guid organizationId, DashboardFilterDto filter, CancellationToken cancellationToken = default);
    Task<MarketingDashboardDto> GetMarketingDashboardAsync(Guid organizationId, DashboardFilterDto filter, CancellationToken cancellationToken = default);
    Task<FinanceDashboardDto> GetFinanceDashboardAsync(Guid organizationId, DashboardFilterDto filter, CancellationToken cancellationToken = default);
    Task<OperationsDashboardDto> GetOperationsDashboardAsync(Guid organizationId, DashboardFilterDto filter, CancellationToken cancellationToken = default);
    Task<TeamPerformanceDashboardDto> GetTeamPerformanceDashboardAsync(Guid organizationId, DashboardFilterDto filter, CancellationToken cancellationToken = default);
}
