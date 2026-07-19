using backend.Modules.Branding.DTOs;
using backend.Modules.Branding.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Branding.Controllers;

[ApiController]
[Route("api/branding")]
[Authorize]
public class BrandingController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandingController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("organizationId")?.Value;
        if (string.IsNullOrEmpty(claim) || !Guid.TryParse(claim, out var orgId))
            throw new UnauthorizedAccessException("Organization context missing.");
        return orgId;
    }

    [HttpGet]
    public async Task<ActionResult<BrandDto>> GetBrand()
    {
        var orgId = GetOrganizationId();
        var brand = await _brandService.GetBrandAsync(orgId);
        return Ok(brand);
    }

    [HttpPut]
    public async Task<ActionResult<BrandDto>> UpdateBrand([FromBody] UpdateBrandDto dto)
    {
        var orgId = GetOrganizationId();
        var brand = await _brandService.UpdateBrandAsync(orgId, dto);
        return Ok(brand);
    }

    [HttpPost("logo")]
    public async Task<ActionResult> UploadLogo(IFormFile file)
    {
        var orgId = GetOrganizationId();
        var url = await _brandService.UploadLogoAsync(orgId, file);
        return Ok(new { LogoUrl = url });
    }

    [HttpPost("favicon")]
    public async Task<ActionResult> UploadFavicon(IFormFile file)
    {
        var orgId = GetOrganizationId();
        var url = await _brandService.UploadFaviconAsync(orgId, file);
        return Ok(new { FaviconUrl = url });
    }
}
