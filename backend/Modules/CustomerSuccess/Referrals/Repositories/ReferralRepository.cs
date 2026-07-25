using backend.Modules.CustomerSuccess.Referrals.Entities;
using backend.Modules.CustomerSuccess.Referrals.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerSuccess.Referrals.Repositories;

public class ReferralRepository : IReferralRepository
{
    private readonly ApplicationDbContext _context;

    public ReferralRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Referral?> GetByIdAsync(Guid orgId, Guid id)
    {
        return await _context.Referrals
            .Include(r => r.ReferrerCustomer)
            .Include(r => r.ReferredCustomer)
            .FirstOrDefaultAsync(r => r.OrganizationId == orgId && r.Id == id);
    }

    public async Task<Referral?> GetByCodeAsync(Guid orgId, string code)
    {
        return await _context.Referrals
            .Include(r => r.ReferrerCustomer)
            .FirstOrDefaultAsync(r => r.OrganizationId == orgId && r.ReferralCode.ToLower() == code.ToLower());
    }

    public async Task<Referral?> GetByReferrerAndReferredAsync(Guid orgId, Guid referrerId, Guid referredId)
    {
        return await _context.Referrals
            .FirstOrDefaultAsync(r => r.OrganizationId == orgId && r.ReferrerCustomerId == referrerId && r.ReferredCustomerId == referredId);
    }

    public async Task<IEnumerable<Referral>> GetByReferrerIdAsync(Guid orgId, Guid referrerId)
    {
        return await _context.Referrals
            .Include(r => r.ReferredCustomer)
            .Where(r => r.OrganizationId == orgId && r.ReferrerCustomerId == referrerId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Referral> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, string? status, string? search, int page, int pageSize)
    {
        var query = _context.Referrals
            .Include(r => r.ReferrerCustomer)
            .Include(r => r.ReferredCustomer)
            .Where(r => r.OrganizationId == orgId)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(r => r.Status.ToLower() == status.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(r => r.ReferralCode.ToLower().Contains(searchLower) ||
                                     (r.ReferrerCustomer != null && r.ReferrerCustomer.Name.ToLower().Contains(searchLower)));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(Referral referral)
    {
        await _context.Referrals.AddAsync(referral);
    }

    public async Task UpdateAsync(Referral referral)
    {
        _context.Referrals.Update(referral);
        await Task.CompletedTask;
    }

    public async Task<int> GetConversionCountAsync(Guid orgId, Guid referrerId)
    {
        return await _context.Referrals
            .CountAsync(r => r.OrganizationId == orgId && r.ReferrerCustomerId == referrerId && (r.Status == "Converted" || r.Status == "Rewarded"));
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
