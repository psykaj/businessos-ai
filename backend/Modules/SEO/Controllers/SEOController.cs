using backend.Modules.SEO.DTOs;
using backend.Modules.SEO.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.SEO.Controllers;

[ApiController]
[Route("api/seo")]
[Authorize]
public class SEOController : ControllerBase
{
    private readonly ISEOService _seoService;

    public SEOController(ISEOService seoService)
    {
        _seoService = seoService;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("organizationId")?.Value;
        if (string.IsNullOrEmpty(claim) || !Guid.TryParse(claim, out var orgId))
            throw new UnauthorizedAccessException("Organization context missing.");
        return orgId;
    }

    [HttpGet]
    public async Task<ActionResult<SEODto>> GetSEO()
    {
        var orgId = GetOrganizationId();
        var seo = await _seoService.GetSEOAsync(orgId);
        return Ok(seo);
    }

    [HttpPut]
    public async Task<ActionResult<SEODto>> UpdateSEO([FromBody] UpdateSEODto dto)
    {
        var orgId = GetOrganizationId();
        var seo = await _seoService.UpdateSEOAsync(orgId, dto);
        return Ok(seo);
    }
}
