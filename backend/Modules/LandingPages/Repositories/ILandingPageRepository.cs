using backend.Entities;

namespace backend.Modules.LandingPages.Repositories;

public interface ILandingPageRepository
{
    Task<IEnumerable<LandingPage>> GetAllByOrganizationIdAsync(Guid organizationId);
    Task<LandingPage?> GetByIdAsync(Guid id, Guid organizationId);
    Task<LandingPage?> GetBySlugAsync(string slug, Guid organizationId);
    Task<LandingPage> AddAsync(LandingPage landingPage);
    Task UpdateAsync(LandingPage landingPage);
    Task DeleteAsync(LandingPage landingPage);
}
