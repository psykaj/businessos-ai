using backend.Modules.AccountsReceivable.Entities;
using backend.Modules.AccountsReceivable.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AccountsReceivable.Repositories;

public class AccountsReceivableRepository : IAccountsReceivableRepository
{
    private readonly ApplicationDbContext _context;

    public AccountsReceivableRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AccountsReceivableRecord?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.AccountsReceivable
            .FirstOrDefaultAsync(a => a.Id == id && a.OrganizationId == organizationId && !a.IsDeleted);
    }

    public async Task<IEnumerable<AccountsReceivableRecord>> GetAllAsync(Guid organizationId, string? status = null)
    {
        var query = _context.AccountsReceivable
            .AsNoTracking()
            .Where(a => a.OrganizationId == organizationId && !a.IsDeleted);

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(a => a.Status == status);
        }

        return await query.OrderBy(a => a.DueDate).ToListAsync();
    }

    public async Task UpdateAsync(AccountsReceivableRecord ar)
    {
        _context.AccountsReceivable.Update(ar);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AccountsReceivableRecord>> GetOverdueReceivablesAsync(Guid organizationId)
    {
        var now = DateTime.UtcNow;
        return await _context.AccountsReceivable
            .Where(a => a.OrganizationId == organizationId && a.BalanceDue > 0 && a.DueDate < now && !a.IsDeleted)
            .ToListAsync();
    }
}
