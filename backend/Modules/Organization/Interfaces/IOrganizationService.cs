using backend.Modules.Organization.DTOs;

namespace backend.Modules.Organization.Interfaces;

public interface IOrganizationService
{
    Task<OrganizationDto> GetOrganizationAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrganizationDto> UpdateOrganizationAsync(Guid id, UpdateOrganizationDto dto, CancellationToken cancellationToken = default);
    Task<string> UploadLogoAsync(Guid id, IFormFile file, CancellationToken cancellationToken = default);
}
