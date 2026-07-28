using backend.Modules.AccountsReceivable.DTOs;

namespace backend.Modules.AccountsReceivable.Interfaces;

public interface IAccountsReceivableService
{
    Task<IEnumerable<AccountsReceivableDto>> GetReceivablesAsync(Guid organizationId, string? status = null);
    Task<AccountsReceivableAgingDto> GetAgingReportAsync(Guid organizationId);
    Task<bool> SendOverdueReminderAsync(Guid id, Guid organizationId, string? customNote);
}
