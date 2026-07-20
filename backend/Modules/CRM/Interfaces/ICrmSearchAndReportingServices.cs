using backend.Modules.CRM.DTOs;

namespace backend.Modules.CRM.Interfaces;

public interface ICrmSearchService
{
    Task<IReadOnlyList<GlobalSearchResultDto>> SearchAsync(string query, Guid organizationId);
}

public interface ICrmReportingService
{
    Task<CrmOverviewDto> GetOverviewAsync(Guid organizationId);
    Task<IReadOnlyList<SalesPerformanceDto>> GetSalesPerformanceAsync(Guid organizationId, DateTime? startDate, DateTime? endDate);
}
