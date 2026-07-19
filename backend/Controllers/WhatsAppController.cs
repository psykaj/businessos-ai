using backend.Entities;
using backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WhatsAppController : ControllerBase
{
    private readonly IWhatsAppService _whatsappService;
    private readonly ILogger<WhatsAppController> _logger;

    public WhatsAppController(IWhatsAppService whatsappService, ILogger<WhatsAppController> logger)
    {
        _whatsappService = whatsappService;
        _logger = logger;
    }

    [HttpGet("settings")]
    public async Task<IActionResult> GetSettings()
    {
        var organizationIdStr = User.FindFirst("OrganizationId")?.Value;
        if (string.IsNullOrEmpty(organizationIdStr) || !Guid.TryParse(organizationIdStr, out var organizationId))
        {
            return Unauthorized();
        }

        var settings = await _whatsappService.GetSettingsAsync(organizationId);
        return Ok(settings ?? new WhatsAppSettings { OrganizationId = organizationId });
    }

    [HttpPost("settings")]
    public async Task<IActionResult> SaveSettings([FromBody] WhatsAppSettings settings)
    {
        var organizationIdStr = User.FindFirst("OrganizationId")?.Value;
        if (string.IsNullOrEmpty(organizationIdStr) || !Guid.TryParse(organizationIdStr, out var organizationId))
        {
            return Unauthorized();
        }

        settings.OrganizationId = organizationId;
        var saved = await _whatsappService.SaveSettingsAsync(settings);
        return Ok(saved);
    }

    [HttpPost("sync-templates")]
    public async Task<IActionResult> SyncTemplates()
    {
        var organizationIdStr = User.FindFirst("OrganizationId")?.Value;
        if (string.IsNullOrEmpty(organizationIdStr) || !Guid.TryParse(organizationIdStr, out var organizationId))
        {
            return Unauthorized();
        }

        try
        {
            var templates = await _whatsappService.SyncTemplatesAsync(organizationId);
            return Ok(templates);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
