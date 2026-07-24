using backend.Modules.AiAgent.Controllers;
using backend.Modules.AiAgent.Conversation.DTOs;
using backend.Modules.AiAgent.Conversation.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.AiAgent.Conversation.Controllers;

[Route("api/ai-agent/conversations")]
public class ConversationController : BaseAiAgentController
{
    private readonly IAiConversationService _conversationService;

    public ConversationController(IAiConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpPost]
    public async Task<ActionResult<ConversationDto>> CreateConversation([FromBody] CreateConversationRequestDto request, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();
        var conversation = await _conversationService.CreateConversationAsync(orgId, userId, request, cancellationToken);
        return CreatedAtAction(nameof(GetConversationById), new { id = conversation.Id }, conversation);
    }

    [HttpGet]
    public async Task<ActionResult> GetConversations(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? status = null,
        CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();
        var (items, totalCount) = await _conversationService.GetConversationsPagedAsync(orgId, userId, page, pageSize, search, status, cancellationToken);
        return Ok(new
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ConversationDto>> GetConversationById(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var conversation = await _conversationService.GetConversationByIdAsync(id, orgId, cancellationToken);
        if (conversation == null) return NotFound(new { Message = "Conversation not found." });
        return Ok(conversation);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ConversationDto>> UpdateConversation(Guid id, [FromBody] UpdateConversationRequestDto request, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var updated = await _conversationService.UpdateConversationAsync(id, orgId, request, cancellationToken);
        if (updated == null) return NotFound(new { Message = "Conversation not found." });
        return Ok(updated);
    }

    [HttpPost("{id:guid}/archive")]
    public async Task<ActionResult> ArchiveConversation(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _conversationService.ArchiveConversationAsync(id, orgId, cancellationToken);
        if (!result) return NotFound(new { Message = "Conversation not found." });
        return Ok(new { Message = "Conversation archived successfully." });
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteConversation(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _conversationService.DeleteConversationAsync(id, orgId, cancellationToken);
        if (!result) return NotFound(new { Message = "Conversation not found." });
        return Ok(new { Message = "Conversation deleted successfully." });
    }

    [HttpGet("{id:guid}/messages")]
    public async Task<ActionResult<List<MessageDto>>> GetMessageHistory(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var messages = await _conversationService.GetMessageHistoryAsync(id, orgId, cancellationToken);
        return Ok(messages);
    }
}
