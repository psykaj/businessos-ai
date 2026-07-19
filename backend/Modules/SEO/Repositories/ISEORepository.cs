using backend.Entities;

namespace backend.Modules.SEO.Repositories;

public interface ISEORepository
{
    Task<SEOSettings?> GetByOrganizationIdAsync(Guid organizationId);
    Task<SEOSettings> AddAsync(SEOSettings settings);
    Task UpdateAsync(SEOSettings settings);
}
