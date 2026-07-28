using backend.Modules.Accounting.Entities;
using backend.Modules.Accounting.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Accounting.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context;

    public AccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(a => a.Id == id && a.OrganizationId == organizationId && !a.IsDeleted);
    }

    public async Task<Account?> GetByCodeAsync(string accountCode, Guid organizationId)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountCode == accountCode && a.OrganizationId == organizationId && !a.IsDeleted);
    }

    public async Task<IEnumerable<Account>> GetAllAsync(Guid organizationId, string? accountType = null, bool activeOnly = false)
    {
        var query = _context.Accounts
            .AsNoTracking()
            .Where(a => a.OrganizationId == organizationId && !a.IsDeleted);

        if (!string.IsNullOrWhiteSpace(accountType))
        {
            query = query.Where(a => a.AccountType == accountType);
        }

        if (activeOnly)
        {
            query = query.Where(a => a.IsActive);
        }

        return await query.OrderBy(a => a.AccountCode).ToListAsync();
    }

    public async Task<Account> AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
        return account;
    }

    public async Task UpdateAsync(Account account)
    {
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Account account)
    {
        account.IsDeleted = true;
        account.DeletedAt = DateTime.UtcNow;
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
    }
}
