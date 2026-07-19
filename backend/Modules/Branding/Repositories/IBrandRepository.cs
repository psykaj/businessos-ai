using backend.Entities;

namespace backend.Modules.Branding.Repositories;

public interface IBrandRepository
{
    Task<Brand?> GetByOrganizationIdAsync(Guid organizationId);
    Task<Brand> AddAsync(Brand brand);
    Task UpdateAsync(Brand brand);
}
