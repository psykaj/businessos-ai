using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Channels.DTOs;
using backend.Modules.CommunicationHub.Channels.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CommunicationHub.Channels.Controllers;

[ApiController]
[Route("api/v1/communication-hub/channels")]
[Authorize]
public class ChannelsController : ControllerBase
{
    private readonly IChannelService _service;

    public ChannelsController(IChannelService service)
    {
        _service = service;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value ?? User.FindFirst("organizationId")?.Value;
        return claim != null && Guid.TryParse(claim, out var orgId) ? orgId : Guid.Empty;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ChannelDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ChannelDto>>> GetAll()
    {
        var orgId = GetOrganizationId();
        if (orgId == Guid.Empty) return Unauthorized(new { Message = "Invalid or missing OrganizationId." });
        
        var channels = await _service.GetChannelsAsync(orgId);
        return Ok(channels);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ChannelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChannelDto>> GetById(Guid id)
    {
        var orgId = GetOrganizationId();
        var channel = await _service.GetChannelByIdAsync(id, orgId);
        if (channel == null) return NotFound();
        return Ok(channel);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ChannelDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ChannelDto>> Create([FromBody] CreateChannelRequest request)
    {
        var orgId = GetOrganizationId();
        if (orgId == Guid.Empty) return Unauthorized();

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var created = await _service.CreateChannelAsync(orgId, request, baseUrl);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ChannelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChannelDto>> Update(Guid id, [FromBody] UpdateChannelRequest request)
    {
        var orgId = GetOrganizationId();
        var updated = await _service.UpdateChannelAsync(id, orgId, request);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var orgId = GetOrganizationId();
        var deleted = await _service.DeleteChannelAsync(id, orgId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
