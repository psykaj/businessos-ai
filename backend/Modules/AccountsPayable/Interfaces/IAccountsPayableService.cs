using backend.Modules.AccountsPayable.DTOs;

namespace backend.Modules.AccountsPayable.Interfaces;

public interface IAccountsPayableService
{
    Task<AccountsPayableDto> CreateBillAsync(Guid organizationId, CreateAccountsPayableDto dto);
    Task<IEnumerable<AccountsPayableDto>> GetPayablesAsync(Guid organizationId, string? status = null);
    Task<AccountsPayableDto?> RecordPaymentAsync(Guid id, Guid organizationId, decimal amountPaid);
    Task<AccountsPayableAgingDto> GetAgingReportAsync(Guid organizationId);
}
