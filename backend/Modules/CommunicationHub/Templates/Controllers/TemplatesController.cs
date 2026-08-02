using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Entities;
using backend.Modules.CommunicationHub.Templates.DTOs;
using backend.Modules.CommunicationHub.Templates.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CommunicationHub.Templates.Controllers;

[ApiController]
[Route("api/v1/communication-hub/templates")]
[Authorize]
public class TemplatesController : ControllerBase
{
    private readonly ITemplateService _service;

    public TemplatesController(ITemplateService service)
    {
        _service = service;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value ?? User.FindFirst("organizationId")?.Value;
        return claim != null && Guid.TryParse(claim, out var orgId) ? orgId : Guid.Empty;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MessageTemplateDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MessageTemplateDto>>> GetAll(
        [FromQuery] CommunicationChannelType? channel = null, 
        [FromQuery] string? category = null)
    {
        var orgId = GetOrganizationId();
        var items = await _service.GetTemplatesAsync(orgId, channel, category);
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MessageTemplateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageTemplateDto>> GetById(Guid id)
    {
        var orgId = GetOrganizationId();
        var item = await _service.GetTemplateByIdAsync(id, orgId);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MessageTemplateDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MessageTemplateDto>> Create([FromBody] CreateTemplateRequest request)
    {
        var orgId = GetOrganizationId();
        if (orgId == Guid.Empty) return Unauthorized();

        var created = await _service.CreateTemplateAsync(orgId, request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(MessageTemplateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageTemplateDto>> Update(Guid id, [FromBody] UpdateTemplateRequest request)
    {
        var orgId = GetOrganizationId();
        var updated = await _service.UpdateTemplateAsync(id, orgId, request);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpPost("{id:guid}/render")]
    [ProducesResponseType(typeof(TemplateRenderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TemplateRenderResponse>> Render(Guid id, [FromBody] TemplateRenderRequest request)
    {
        var orgId = GetOrganizationId();
        var rendered = await _service.RenderTemplateAsync(id, orgId, request.Variables);
        if (rendered == null) return NotFound();
        return Ok(rendered);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var orgId = GetOrganizationId();
        var deleted = await _service.DeleteTemplateAsync(id, orgId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
