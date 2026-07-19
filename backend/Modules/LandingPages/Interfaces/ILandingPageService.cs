using backend.Modules.LandingPages.DTOs;

namespace backend.Modules.LandingPages.Interfaces;

public interface ILandingPageService
{
    Task<IEnumerable<LandingPageDto>> GetPagesAsync(Guid organizationId);
    Task<LandingPageDto> GetPageByIdAsync(Guid organizationId, Guid id);
    Task<LandingPageDto> GetPageBySlugAsync(Guid organizationId, string slug);
    Task<LandingPageDto> CreatePageAsync(Guid organizationId, CreateLandingPageDto dto);
    Task<LandingPageDto> UpdatePageAsync(Guid organizationId, Guid id, UpdateLandingPageDto dto);
    Task DeletePageAsync(Guid organizationId, Guid id);
}
