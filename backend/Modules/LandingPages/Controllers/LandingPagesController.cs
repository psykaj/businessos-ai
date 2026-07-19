using backend.Modules.LandingPages.DTOs;
using backend.Modules.LandingPages.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.LandingPages.Controllers;

[ApiController]
[Route("api/landing-pages")]
[Authorize]
public class LandingPagesController : ControllerBase
{
    private readonly ILandingPageService _pageService;

    public LandingPagesController(ILandingPageService pageService)
    {
        _pageService = pageService;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("organizationId")?.Value;
        if (string.IsNullOrEmpty(claim) || !Guid.TryParse(claim, out var orgId))
            throw new UnauthorizedAccessException("Organization context missing.");
        return orgId;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LandingPageDto>>> GetPages()
    {
        var orgId = GetOrganizationId();
        var pages = await _pageService.GetPagesAsync(orgId);
        return Ok(pages);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LandingPageDto>> GetPage(Guid id)
    {
        var orgId = GetOrganizationId();
        var page = await _pageService.GetPageByIdAsync(orgId, id);
        return Ok(page);
    }

    [HttpGet("slug/{slug}")]
    [AllowAnonymous] // Allow public access by slug
    public async Task<ActionResult<LandingPageDto>> GetPageBySlug(string slug, [FromQuery] Guid orgId)
    {
        var page = await _pageService.GetPageBySlugAsync(orgId, slug);
        if (page.Status != "Published") return NotFound("Page is not published.");
        return Ok(page);
    }

    [HttpPost]
    public async Task<ActionResult<LandingPageDto>> CreatePage([FromBody] CreateLandingPageDto dto)
    {
        var orgId = GetOrganizationId();
        var page = await _pageService.CreatePageAsync(orgId, dto);
        return Ok(page);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LandingPageDto>> UpdatePage(Guid id, [FromBody] UpdateLandingPageDto dto)
    {
        var orgId = GetOrganizationId();
        var page = await _pageService.UpdatePageAsync(orgId, id, dto);
        return Ok(page);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePage(Guid id)
    {
        var orgId = GetOrganizationId();
        await _pageService.DeletePageAsync(orgId, id);
        return NoContent();
    }
}
