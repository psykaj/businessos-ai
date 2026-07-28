using backend.Modules.Accounting.DTOs;

namespace backend.Modules.Accounting.Interfaces;

public interface IAccountingService
{
    Task<AccountDto> CreateAccountAsync(Guid organizationId, CreateAccountDto dto);
    Task<AccountDto?> UpdateAccountAsync(Guid id, Guid organizationId, UpdateAccountDto dto);
    Task<bool> DeleteAccountAsync(Guid id, Guid organizationId);
    Task<AccountDto?> GetAccountByIdAsync(Guid id, Guid organizationId);
    Task<IEnumerable<AccountDto>> GetChartOfAccountsAsync(Guid organizationId, string? accountType = null, bool activeOnly = false);
    Task SeedDefaultChartOfAccountsAsync(Guid organizationId);
}
