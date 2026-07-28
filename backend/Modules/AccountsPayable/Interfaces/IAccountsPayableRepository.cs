using backend.Modules.AccountsPayable.Entities;

namespace backend.Modules.AccountsPayable.Interfaces;

public interface IAccountsPayableRepository
{
    Task<AccountsPayableRecord?> GetByIdAsync(Guid id, Guid organizationId);
    Task<IEnumerable<AccountsPayableRecord>> GetAllAsync(Guid organizationId, string? status = null);
    Task<AccountsPayableRecord> AddAsync(AccountsPayableRecord ap);
    Task UpdateAsync(AccountsPayableRecord ap);
    Task DeleteAsync(AccountsPayableRecord ap);
}
