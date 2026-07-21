using backend.Modules.Forms.DTOs;
using backend.Modules.Forms.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace backend.Modules.Forms.Controllers;

[ApiController]
[Route("api/public/forms")]
public class PublicFormsController : ControllerBase
{
    private readonly IFormService _formService;

    public PublicFormsController(IFormService formService)
    {
        _formService = formService;
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetFormBySlug(string slug, CancellationToken cancellationToken)
    {
        var form = await _formService.GetFormBySlugAsync(slug, cancellationToken);
        return Ok(form);
    }

    [HttpPost("{id}/submit")]
    public async Task<IActionResult> SubmitForm(Guid id, [FromBody] JsonElement rawData, [FromQuery] string source = "Website", CancellationToken cancellationToken = default)
    {
        var device = Request.Headers["User-Agent"].ToString();
        var browser = Request.Headers["Sec-CH-UA"].ToString() ?? device;
        
        // Find the organization ID from the form itself
        // In a real scenario, this requires a public endpoint that first looks up the form
        // We will fetch the form without org filter to get its OrganizationId
        // For security, the service layer ensures the form is Published
        
        var dto = new SubmitFormDto 
        { 
            SubmittedData = rawData.GetRawText(),
            Source = source
        };

        await _formService.PublicSubmitFormAsync(id, dto, device, browser, cancellationToken);
        return Accepted(new { message = "Form submitted successfully" });
    }
}
