using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Messages.DTOs;
using backend.Modules.CommunicationHub.Messages.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CommunicationHub.Messages.Controllers;

[ApiController]
[Route("api/v1/communication-hub/messages")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _service;

    public MessagesController(IMessageService service)
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

    [HttpGet("conversation/{conversationId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<MessageDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetByConversation(Guid conversationId)
    {
        var orgId = GetOrganizationId();
        var messages = await _service.GetMessagesAsync(conversationId, orgId);
        return Ok(messages);
    }

    [HttpPost("send")]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageDto>> Send([FromBody] SendMessageRequest request)
    {
        var orgId = GetOrganizationId();
        var (userId, userName) = GetCurrentUserInfo();

        var message = await _service.SendMessageAsync(orgId, request, userId, userName);
        if (message == null) return NotFound(new { Message = "Conversation not found or inaccessible." });
        return Ok(message);
    }

    [HttpPost("note")]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageDto>> AddNote([FromBody] AddInternalNoteRequest request)
    {
        var orgId = GetOrganizationId();
        var (userId, userName) = GetCurrentUserInfo();

        var note = await _service.AddInternalNoteAsync(orgId, request, userId, userName);
        if (note == null) return NotFound(new { Message = "Conversation not found." });
        return Ok(note);
    }

    [HttpPost("mark-read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> MarkAsRead([FromBody] MarkMessagesReadRequest request)
    {
        var orgId = GetOrganizationId();
        await _service.MarkConversationMessagesAsReadAsync(request.ConversationId, orgId);
        return Ok(new { Success = true });
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var orgId = GetOrganizationId();
        var deleted = await _service.DeleteMessageAsync(id, orgId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
