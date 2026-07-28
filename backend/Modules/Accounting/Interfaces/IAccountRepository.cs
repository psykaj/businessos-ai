using backend.Modules.Accounting.Entities;

namespace backend.Modules.Accounting.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id, Guid organizationId);
    Task<Account?> GetByCodeAsync(string accountCode, Guid organizationId);
    Task<IEnumerable<Account>> GetAllAsync(Guid organizationId, string? accountType = null, bool activeOnly = false);
    Task<Account> AddAsync(Account account);
    Task UpdateAsync(Account account);
    Task DeleteAsync(Account account);
}
