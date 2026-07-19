using backend.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.LandingPages.Repositories;

public class LandingPageRepository : ILandingPageRepository
{
    private readonly ApplicationDbContext _context;

    public LandingPageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LandingPage>> GetAllByOrganizationIdAsync(Guid organizationId)
    {
        return await _context.LandingPages
            .Include(p => p.Sections)
            .Where(p => p.OrganizationId == organizationId && !p.IsDeleted)
            .ToListAsync();
    }

    public async Task<LandingPage?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.LandingPages
            .Include(p => p.Sections)
            .FirstOrDefaultAsync(p => p.Id == id && p.OrganizationId == organizationId && !p.IsDeleted);
    }

    public async Task<LandingPage?> GetBySlugAsync(string slug, Guid organizationId)
    {
        return await _context.LandingPages
            .Include(p => p.Sections)
            .FirstOrDefaultAsync(p => p.Slug == slug && p.OrganizationId == organizationId && !p.IsDeleted);
    }

    public async Task<LandingPage> AddAsync(LandingPage landingPage)
    {
        await _context.LandingPages.AddAsync(landingPage);
        await _context.SaveChangesAsync();
        return landingPage;
    }

    public async Task UpdateAsync(LandingPage landingPage)
    {
        _context.LandingPages.Update(landingPage);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(LandingPage landingPage)
    {
        landingPage.IsDeleted = true;
        landingPage.DeletedAt = DateTime.UtcNow;
        _context.LandingPages.Update(landingPage);
        await _context.SaveChangesAsync();
    }
}
