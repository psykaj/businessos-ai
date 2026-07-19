using backend.Modules.Branding.DTOs;
using Microsoft.AspNetCore.Http;

namespace backend.Modules.Branding.Interfaces;

public interface IBrandService
{
    Task<BrandDto> GetBrandAsync(Guid organizationId);
    Task<BrandDto> UpdateBrandAsync(Guid organizationId, UpdateBrandDto dto);
    Task<string> UploadLogoAsync(Guid organizationId, IFormFile file);
    Task<string> UploadFaviconAsync(Guid organizationId, IFormFile file);
}
