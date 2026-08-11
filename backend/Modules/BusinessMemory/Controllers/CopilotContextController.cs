using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Modules.BusinessMemory.DTOs;
using backend.Modules.BusinessMemory.Interfaces;
using System.Linq;

namespace backend.Modules.BusinessMemory.Controllers;

[ApiController]
[Route("api/copilot/context")]
[Authorize]
public class CopilotContextController : ControllerBase
{
    private readonly IMemoryRetrievalService _retrievalService;
    private readonly IMemorySummarizer _summarizer;

    public CopilotContextController(
        IMemoryRetrievalService retrievalService, 
        IMemorySummarizer summarizer)
    {
        _retrievalService = retrievalService;
        _summarizer = summarizer;
    }

    private Guid GetOrganizationId()
    {
        var orgIdClaim = User.FindFirst("organizationId")?.Value;
        if (Guid.TryParse(orgIdClaim, out var orgId)) return orgId;

        var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(subClaim)) return Guid.Parse("00000000-0000-0000-0000-000000000001");

        throw new UnauthorizedAccessException("Organization ID not found in token context.");
    }

    [HttpPost]
    public async Task<ActionResult<CopilotContextResponseDto>> GetContext([FromBody] CopilotContextRequestDto request, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();

        var memories = await _retrievalService.RetrieveRelevantMemoriesAsync(
            orgId, 
            request.Question, 
            request.EntityType, 
            request.EntityId, 
            10, 
            cancellationToken);

        // Map internal entities to DTOs for the response
        var memoryDtos = memories.Select(m => new MemoryDto
        {
            Id = m.Id,
            OrganizationId = m.OrganizationId,
            MemoryType = m.MemoryType,
            Title = m.Title,
            Content = m.Content,
            StructuredData = m.StructuredData,
            SourceModule = m.SourceModule,
            SourceEntityId = m.SourceEntityId,
            Importance = m.Importance,
            Confidence = m.Confidence,
            ExpiresAt = m.ExpiresAt,
            IsActive = m.IsActive,
            CreatedAt = m.CreatedAt,
            UpdatedAt = m.UpdatedAt
        }).ToList();

        var response = new CopilotContextResponseDto
        {
            RelevantContext = "Compiled context based on memory relevance...",
            CurrentData = new { Note = "Current real-time data would be injected here from specific modules." },
            RelevantMemories = memoryDtos,
            SuggestedSources = memoryDtos.Select(m => m.SourceModule).Distinct()
        };

        return Ok(response);
    }
}
