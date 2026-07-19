using backend.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.SEO.Repositories;

public class SEORepository : ISEORepository
{
    private readonly ApplicationDbContext _context;

    public SEORepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SEOSettings?> GetByOrganizationIdAsync(Guid organizationId)
    {
        return await _context.SEOSettings
            .FirstOrDefaultAsync(s => s.OrganizationId == organizationId && !s.IsDeleted);
    }

    public async Task<SEOSettings> AddAsync(SEOSettings settings)
    {
        await _context.SEOSettings.AddAsync(settings);
        await _context.SaveChangesAsync();
        return settings;
    }

    public async Task UpdateAsync(SEOSettings settings)
    {
        _context.SEOSettings.Update(settings);
        await _context.SaveChangesAsync();
    }
}
