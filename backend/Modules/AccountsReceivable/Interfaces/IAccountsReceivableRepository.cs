using backend.Modules.AccountsReceivable.Entities;

namespace backend.Modules.AccountsReceivable.Interfaces;

public interface IAccountsReceivableRepository
{
    Task<AccountsReceivableRecord?> GetByIdAsync(Guid id, Guid organizationId);
    Task<IEnumerable<AccountsReceivableRecord>> GetAllAsync(Guid organizationId, string? status = null);
    Task UpdateAsync(AccountsReceivableRecord ar);
    Task<IEnumerable<AccountsReceivableRecord>> GetOverdueReceivablesAsync(Guid organizationId);
}
