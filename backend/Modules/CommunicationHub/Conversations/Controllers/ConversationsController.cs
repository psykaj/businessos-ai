using System;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Conversations.DTOs;
using backend.Modules.CommunicationHub.Conversations.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CommunicationHub.Conversations.Controllers;

[ApiController]
[Route("api/v1/communication-hub/conversations")]
[Authorize]
public class ConversationsController : ControllerBase
{
    private readonly IConversationService _service;

    public ConversationsController(IConversationService service)
    {
        _service = service;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value ?? User.FindFirst("organizationId")?.Value;
        return claim != null && Guid.TryParse(claim, out var orgId) ? orgId : Guid.Empty;
    }

    private (Guid UserId, string UserName) GetCurrentUserInfo()
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? User.FindFirst("name")?.Value ?? "Agent";
        var userId = userIdStr != null && Guid.TryParse(userIdStr, out var id) ? id : Guid.Empty;
        return (userId, userName);
    }

    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromQuery] ConversationFilterDto filter)
    {
        var orgId = GetOrganizationId();
        if (orgId == Guid.Empty) return Unauthorized();

        var (items, total) = await _service.GetConversationsAsync(orgId, filter);
        return Ok(new { Items = items, TotalCount = total, PageNumber = filter.PageNumber, PageSize = filter.PageSize });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ConversationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConversationDto>> GetById(Guid id)
    {
        var orgId = GetOrganizationId();
        var item = await _service.GetConversationByIdAsync(id, orgId);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ConversationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ConversationDto>> Create([FromBody] CreateConversationRequest request)
    {
        var orgId = GetOrganizationId();
        if (orgId == Guid.Empty) return Unauthorized();

        var (userId, userName) = GetCurrentUserInfo();
        var created = await _service.CreateConversationAsync(orgId, request, userId, userName);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(ConversationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConversationDto>> UpdateStatus(Guid id, [FromBody] UpdateConversationStatusRequest request)
    {
        var orgId = GetOrganizationId();
        var updated = await _service.UpdateStatusAsync(id, orgId, request.Status);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpPost("{id:guid}/assign")]
    [ProducesResponseType(typeof(ConversationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConversationDto>> Assign(Guid id, [FromBody] AssignConversationRequest request)
    {
        var orgId = GetOrganizationId();
        var (currentUserId, currentUserName) = GetCurrentUserInfo();

        var assigned = await _service.AssignConversationAsync(
            id, orgId, request.AssignedToUserId, request.AssignedToUserName, currentUserId, currentUserName, request.AssignmentReason);

        if (assigned == null) return NotFound();
        return Ok(assigned);
    }

    [HttpPatch("{id:guid}/tags")]
    [ProducesResponseType(typeof(ConversationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConversationDto>> UpdateTags(Guid id, [FromBody] AddTagsRequest request)
    {
        var orgId = GetOrganizationId();
        var updated = await _service.UpdateTagsAsync(id, orgId, request.Tags);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var orgId = GetOrganizationId();
        var deleted = await _service.DeleteConversationAsync(id, orgId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
