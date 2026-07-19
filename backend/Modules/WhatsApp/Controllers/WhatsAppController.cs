using backend.Entities;
using backend.Modules.WhatsApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.WhatsApp.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WhatsAppController : ControllerBase
{
    private readonly IWhatsAppRepository _whatsAppRepository;

    public WhatsAppController(IWhatsAppRepository whatsAppRepository)
    {
        _whatsAppRepository = whatsAppRepository;
    }

    private Guid GetOrganizationId()
    {
        var orgClaim = User.FindFirst("organizationId")?.Value;
        return Guid.TryParse(orgClaim, out var orgId) ? orgId : Guid.Empty;
    }

    [HttpGet("settings")]
    public async Task<IActionResult> GetSettings()
    {
        var settings = await _whatsAppRepository.GetSettingsAsync(GetOrganizationId());
        return Ok(settings);
    }

    [HttpPost("settings")]
    public async Task<IActionResult> SaveSettings([FromBody] WhatsAppSettings settings)
    {
        settings.OrganizationId = GetOrganizationId();
        var saved = await _whatsAppRepository.SaveSettingsAsync(settings);
        return Ok(saved);
    }

    [HttpGet("templates")]
    public async Task<IActionResult> GetTemplates()
    {
        var templates = await _whatsAppRepository.GetTemplatesAsync(GetOrganizationId());
        return Ok(templates);
    }
}
