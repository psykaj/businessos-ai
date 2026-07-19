using backend.Entities;
using backend.Modules.AI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Modules.AI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AIController : ControllerBase
{
    private readonly IAIService _aiService;
    private readonly IAIRepository _aiRepository;

    public AIController(IAIService aiService, IAIRepository aiRepository)
    {
        _aiService = aiService;
        _aiRepository = aiRepository;
    }

    private Guid GetOrganizationId()
    {
        var orgClaim = User.FindFirst("OrganizationId")?.Value;
        return Guid.TryParse(orgClaim, out var orgId) ? orgId : Guid.Empty;
    }

    private string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }

    [HttpGet("conversations")]
    public async Task<IActionResult> GetConversations()
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();
        var conversations = await _aiRepository.GetConversationsAsync(orgId, userId);
        return Ok(conversations);
    }

    [HttpPost("conversations")]
    public async Task<IActionResult> CreateConversation([FromBody] CreateConversationDto dto)
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();
        var conversation = new AIConversation
        {
            OrganizationId = orgId,
            UserId = userId,
            Title = dto.Title,
            Provider = dto.Provider ?? "OpenAI",
            Model = dto.Model ?? "gpt-4o"
        };

        var created = await _aiRepository.CreateConversationAsync(conversation);
        return CreatedAtAction(nameof(GetConversation), new { id = created.Id }, created);
    }

    [HttpGet("conversations/{id}")]
    public async Task<IActionResult> GetConversation(Guid id)
    {
        var orgId = GetOrganizationId();
        var conversation = await _aiRepository.GetConversationAsync(orgId, id);
        if (conversation == null) return NotFound();
        return Ok(conversation);
    }

    [HttpPost("conversations/{id}/messages")]
    public async Task<IActionResult> SendMessage(Guid id, [FromBody] SendMessageDto dto)
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();
        
        try
        {
            var updatedConversation = await _aiService.ProcessConversationTurnAsync(orgId, userId, id, dto.Message);
            return Ok(updatedConversation);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

public class CreateConversationDto
{
    public string Title { get; set; } = string.Empty;
    public string? Provider { get; set; }
    public string? Model { get; set; }
}

public class SendMessageDto
{
    public string Message { get; set; } = string.Empty;
}
