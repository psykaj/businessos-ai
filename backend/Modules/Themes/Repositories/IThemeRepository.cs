using backend.Entities;

namespace backend.Modules.Themes.Repositories;

public interface IThemeRepository
{
    Task<IEnumerable<Theme>> GetAllByOrganizationIdAsync(Guid organizationId);
    Task<Theme?> GetByIdAsync(Guid id, Guid organizationId);
    Task<Theme> AddAsync(Theme theme);
    Task UpdateAsync(Theme theme);
    Task DeleteAsync(Theme theme);
}
