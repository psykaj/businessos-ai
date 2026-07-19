using backend.Modules.Themes.DTOs;
using backend.Modules.Themes.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Themes.Controllers;

[ApiController]
[Route("api/themes")]
[Authorize]
public class ThemesController : ControllerBase
{
    private readonly IThemeService _themeService;

    public ThemesController(IThemeService themeService)
    {
        _themeService = themeService;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("organizationId")?.Value;
        if (string.IsNullOrEmpty(claim) || !Guid.TryParse(claim, out var orgId))
            throw new UnauthorizedAccessException("Organization context missing.");
        return orgId;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ThemeDto>>> GetThemes()
    {
        var orgId = GetOrganizationId();
        var themes = await _themeService.GetThemesAsync(orgId);
        return Ok(themes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ThemeDto>> GetTheme(Guid id)
    {
        var orgId = GetOrganizationId();
        var theme = await _themeService.GetThemeByIdAsync(orgId, id);
        return Ok(theme);
    }

    [HttpPost]
    public async Task<ActionResult<ThemeDto>> CreateTheme([FromBody] CreateThemeDto dto)
    {
        var orgId = GetOrganizationId();
        var theme = await _themeService.CreateThemeAsync(orgId, dto);
        return Ok(theme);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ThemeDto>> UpdateTheme(Guid id, [FromBody] UpdateThemeDto dto)
    {
        var orgId = GetOrganizationId();
        var theme = await _themeService.UpdateThemeAsync(orgId, id, dto);
        return Ok(theme);
    }

    [HttpPut("{id}/default")]
    public async Task<ActionResult<ThemeDto>> SetDefault(Guid id)
    {
        var orgId = GetOrganizationId();
        var theme = await _themeService.SetDefaultThemeAsync(orgId, id);
        return Ok(theme);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTheme(Guid id)
    {
        var orgId = GetOrganizationId();
        await _themeService.DeleteThemeAsync(orgId, id);
        return NoContent();
    }
}
