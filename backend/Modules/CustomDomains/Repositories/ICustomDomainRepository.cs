using backend.Entities;

namespace backend.Modules.CustomDomains.Repositories;

public interface ICustomDomainRepository
{
    Task<IEnumerable<CustomDomain>> GetAllByOrganizationIdAsync(Guid organizationId);
    Task<CustomDomain?> GetByIdAsync(Guid id, Guid organizationId);
    Task<CustomDomain?> GetByDomainAsync(string domain);
    Task<CustomDomain> AddAsync(CustomDomain domain);
    Task UpdateAsync(CustomDomain domain);
    Task DeleteAsync(CustomDomain domain);
}
