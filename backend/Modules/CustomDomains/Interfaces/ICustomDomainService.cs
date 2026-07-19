using backend.Modules.CustomDomains.DTOs;

namespace backend.Modules.CustomDomains.Interfaces;

public interface ICustomDomainService
{
    Task<IEnumerable<CustomDomainDto>> GetDomainsAsync(Guid organizationId);
    Task<CustomDomainDto> AddDomainAsync(Guid organizationId, CreateCustomDomainDto dto);
    Task<CustomDomainDto> SetPrimaryDomainAsync(Guid organizationId, Guid domainId);
    Task DeleteDomainAsync(Guid organizationId, Guid domainId);
    Task<bool> VerifyDomainAsync(Guid organizationId, Guid domainId);
    Task<object> GetDnsInstructionsAsync(Guid organizationId, Guid domainId);
}
