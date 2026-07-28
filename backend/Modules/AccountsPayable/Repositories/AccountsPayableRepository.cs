using backend.Modules.AccountsPayable.Entities;
using backend.Modules.AccountsPayable.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AccountsPayable.Repositories;

public class AccountsPayableRepository : IAccountsPayableRepository
{
    private readonly ApplicationDbContext _context;

    public AccountsPayableRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AccountsPayableRecord?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.AccountsPayable
            .FirstOrDefaultAsync(a => a.Id == id && a.OrganizationId == organizationId && !a.IsDeleted);
    }

    public async Task<IEnumerable<AccountsPayableRecord>> GetAllAsync(Guid organizationId, string? status = null)
    {
        var query = _context.AccountsPayable
            .AsNoTracking()
            .Where(a => a.OrganizationId == organizationId && !a.IsDeleted);

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(a => a.Status == status);
        }

        return await query.OrderBy(a => a.DueDate).ToListAsync();
    }

    public async Task<AccountsPayableRecord> AddAsync(AccountsPayableRecord ap)
    {
        await _context.AccountsPayable.AddAsync(ap);
        await _context.SaveChangesAsync();
        return ap;
    }

    public async Task UpdateAsync(AccountsPayableRecord ap)
    {
        _context.AccountsPayable.Update(ap);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(AccountsPayableRecord ap)
    {
        ap.IsDeleted = true;
        ap.DeletedAt = DateTime.UtcNow;
        _context.AccountsPayable.Update(ap);
        await _context.SaveChangesAsync();
    }
}
