using backend.Modules.CustomerSuccess.Loyalty.Entities;
using backend.Modules.CustomerSuccess.Loyalty.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerSuccess.Loyalty.Repositories;

public class LoyaltyRepository : ILoyaltyRepository
{
    private readonly ApplicationDbContext _context;

    public LoyaltyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LoyaltyProgram?> GetProgramByIdAsync(Guid orgId, Guid id)
    {
        return await _context.LoyaltyPrograms
            .FirstOrDefaultAsync(p => p.OrganizationId == orgId && p.Id == id);
    }

    public async Task<LoyaltyProgram?> GetDefaultProgramAsync(Guid orgId)
    {
        return await _context.LoyaltyPrograms
            .FirstOrDefaultAsync(p => p.OrganizationId == orgId && p.IsDefault && p.Status == "Active")
            ?? await _context.LoyaltyPrograms.FirstOrDefaultAsync(p => p.OrganizationId == orgId && p.Status == "Active");
    }

    public async Task<IEnumerable<LoyaltyProgram>> GetProgramsAsync(Guid orgId)
    {
        return await _context.LoyaltyPrograms
            .Where(p => p.OrganizationId == orgId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task AddProgramAsync(LoyaltyProgram program)
    {
        await _context.LoyaltyPrograms.AddAsync(program);
    }

    public async Task UpdateProgramAsync(LoyaltyProgram program)
    {
        _context.LoyaltyPrograms.Update(program);
        await Task.CompletedTask;
    }

    public async Task<int> GetCustomerBalanceAsync(Guid orgId, Guid customerId, Guid programId)
    {
        var latestTx = await _context.LoyaltyTransactions
            .Where(t => t.OrganizationId == orgId && t.CustomerId == customerId && t.ProgramId == programId)
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync();

        return latestTx?.Balance ?? 0;
    }

    public async Task AddTransactionAsync(LoyaltyTransaction transaction)
    {
        await _context.LoyaltyTransactions.AddAsync(transaction);
    }

    public async Task<(IEnumerable<LoyaltyTransaction> Items, int TotalCount)> GetTransactionsPagedAsync(
        Guid orgId, Guid? customerId, Guid? programId, int page, int pageSize)
    {
        var query = _context.LoyaltyTransactions
            .Include(t => t.Customer)
            .Include(t => t.Program)
            .Where(t => t.OrganizationId == orgId)
            .AsNoTracking();

        if (customerId.HasValue)
        {
            query = query.Where(t => t.CustomerId == customerId.Value);
        }

        if (programId.HasValue)
        {
            query = query.Where(t => t.ProgramId == programId.Value);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<LoyaltyTransaction>> GetExpiredPointsAsync(Guid orgId, DateTime cutoffDate)
    {
        return await _context.LoyaltyTransactions
            .Where(t => t.OrganizationId == orgId && t.ExpiryDate.HasValue && t.ExpiryDate.Value <= cutoffDate && t.TransactionType == "Earned")
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
