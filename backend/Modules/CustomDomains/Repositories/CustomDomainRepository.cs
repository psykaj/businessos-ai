using backend.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomDomains.Repositories;

public class CustomDomainRepository : ICustomDomainRepository
{
    private readonly ApplicationDbContext _context;

    public CustomDomainRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomDomain>> GetAllByOrganizationIdAsync(Guid organizationId)
    {
        return await _context.CustomDomains
            .Where(d => d.OrganizationId == organizationId && !d.IsDeleted)
            .ToListAsync();
    }

    public async Task<CustomDomain?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.CustomDomains
            .FirstOrDefaultAsync(d => d.Id == id && d.OrganizationId == organizationId && !d.IsDeleted);
    }

    public async Task<CustomDomain?> GetByDomainAsync(string domain)
    {
        return await _context.CustomDomains
            .FirstOrDefaultAsync(d => d.Domain == domain && !d.IsDeleted);
    }

    public async Task<CustomDomain> AddAsync(CustomDomain domain)
    {
        await _context.CustomDomains.AddAsync(domain);
        await _context.SaveChangesAsync();
        return domain;
    }

    public async Task UpdateAsync(CustomDomain domain)
    {
        _context.CustomDomains.Update(domain);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CustomDomain domain)
    {
        domain.IsDeleted = true;
        domain.DeletedAt = DateTime.UtcNow;
        _context.CustomDomains.Update(domain);
        await _context.SaveChangesAsync();
    }
}
