using backend.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Themes.Repositories;

public class ThemeRepository : IThemeRepository
{
    private readonly ApplicationDbContext _context;

    public ThemeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Theme>> GetAllByOrganizationIdAsync(Guid organizationId)
    {
        return await _context.Themes
            .Where(t => t.OrganizationId == organizationId && !t.IsDeleted)
            .ToListAsync();
    }

    public async Task<Theme?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.Themes
            .FirstOrDefaultAsync(t => t.Id == id && t.OrganizationId == organizationId && !t.IsDeleted);
    }

    public async Task<Theme> AddAsync(Theme theme)
    {
        await _context.Themes.AddAsync(theme);
        await _context.SaveChangesAsync();
        return theme;
    }

    public async Task UpdateAsync(Theme theme)
    {
        _context.Themes.Update(theme);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Theme theme)
    {
        theme.IsDeleted = true;
        theme.DeletedAt = DateTime.UtcNow;
        _context.Themes.Update(theme);
        await _context.SaveChangesAsync();
    }
}
