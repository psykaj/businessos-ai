using backend.Modules.CashFlow.DTOs;

namespace backend.Modules.CashFlow.Interfaces;

public interface ICashFlowEngine
{
    Task<CashFlowSummaryDto> GetSummaryAsync(Guid organizationId, DateTime? startDate = null, DateTime? endDate = null);
    Task<IEnumerable<MonthlyCashFlowPointDto>> GetMonthlyCashFlowAsync(Guid organizationId, int months = 6);
    Task<CashFlowForecastDto> GetForecastAsync(Guid organizationId);
    Task<CashFlowEntryDto> AddEntryAsync(Guid organizationId, CreateCashFlowEntryDto dto);
}
