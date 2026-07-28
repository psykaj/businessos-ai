using backend.Modules.Finance.DTOs;

namespace backend.Modules.Finance.Interfaces;

public interface IFinanceOverviewService
{
    Task<FinanceOverviewDto> GetOverviewAsync(Guid organizationId);
}
