using backend.Modules.SEO.DTOs;

namespace backend.Modules.SEO.Interfaces;

public interface ISEOService
{
    Task<SEODto> GetSEOAsync(Guid organizationId);
    Task<SEODto> UpdateSEOAsync(Guid organizationId, UpdateSEODto dto);
}
