using backend.Modules.CustomerSuccess.CustomerHealth.Entities;
using backend.Modules.CustomerSuccess.CustomerHealth.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerSuccess.CustomerHealth.Repositories;

public class CustomerHealthRepository : ICustomerHealthRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerHealthRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Entities.CustomerHealth?> GetByCustomerIdAsync(Guid orgId, Guid customerId)
    {
        return await _context.CustomerHealths
            .Include(h => h.Customer)
            .FirstOrDefaultAsync(h => h.OrganizationId == orgId && h.CustomerId == customerId);
    }

    public async Task<Entities.CustomerHealth?> GetByIdAsync(Guid orgId, Guid id)
    {
        return await _context.CustomerHealths
            .Include(h => h.Customer)
            .FirstOrDefaultAsync(h => h.OrganizationId == orgId && h.Id == id);
    }

    public async Task<(IEnumerable<Entities.CustomerHealth> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, string? riskLevel, string? search, int page, int pageSize, string? sortBy, bool descending)
    {
        var query = _context.CustomerHealths
            .Include(h => h.Customer)
            .Where(h => h.OrganizationId == orgId)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(riskLevel))
        {
            query = query.Where(h => h.RiskLevel.ToLower() == riskLevel.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(h => h.Customer != null && h.Customer.Name.ToLower().Contains(searchLower));
        }

        var totalCount = await query.CountAsync();

        query = sortBy?.ToLower() switch
        {
            "healthscore" => descending ? query.OrderByDescending(h => h.HealthScore) : query.OrderBy(h => h.HealthScore),
            "lifetimevalue" => descending ? query.OrderByDescending(h => h.LifetimeValue) : query.OrderBy(h => h.LifetimeValue),
            "lastinteractiondate" => descending ? query.OrderByDescending(h => h.LastInteractionDate) : query.OrderBy(h => h.LastInteractionDate),
            _ => descending ? query.OrderByDescending(h => h.UpdatedAt) : query.OrderBy(h => h.UpdatedAt)
        };

        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<IEnumerable<Entities.CustomerHealth>> GetHighRiskCustomersAsync(Guid orgId)
    {
        return await _context.CustomerHealths
            .Include(h => h.Customer)
            .Where(h => h.OrganizationId == orgId && (h.RiskLevel == "High Risk" || h.HealthScore < 40))
            .ToListAsync();
    }

    public async Task AddAsync(Entities.CustomerHealth health)
    {
        await _context.CustomerHealths.AddAsync(health);
    }

    public async Task UpdateAsync(Entities.CustomerHealth health)
    {
        _context.CustomerHealths.Update(health);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
