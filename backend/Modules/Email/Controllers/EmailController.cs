using backend.Entities;
using backend.Modules.Email.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Email.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailRepository _emailRepository;
    private readonly IEmailService _emailService;

    public EmailController(IEmailRepository emailRepository, IEmailService emailService)
    {
        _emailRepository = emailRepository;
        _emailService = emailService;
    }

    private Guid GetOrganizationId()
    {
        var orgClaim = User.FindFirst("OrganizationId")?.Value;
        return Guid.TryParse(orgClaim, out var orgId) ? orgId : Guid.Empty;
    }

    [HttpGet("templates")]
    public async Task<IActionResult> GetTemplates()
    {
        var templates = await _emailRepository.GetTemplatesAsync(GetOrganizationId());
        return Ok(templates);
    }

    [HttpPost("templates")]
    public async Task<IActionResult> CreateTemplate([FromBody] EmailTemplate template)
    {
        template.OrganizationId = GetOrganizationId();
        var created = await _emailRepository.AddTemplateAsync(template);
        return Ok(created);
    }
}
