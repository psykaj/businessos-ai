using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.AiRecommendations.DTOs;
using backend.Modules.AiRecommendations.Entities;
using backend.Modules.AiRecommendations.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.AiRecommendations.Controllers;

[ApiController]
[Route("api/v1/ai-recommendations")]
[Authorize]
public class AiRecommendationsController : ControllerBase
{
    private readonly IAiRecommendationRepository _repository;

    public AiRecommendationsController(IAiRecommendationRepository repository)
    {
        _repository = repository;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value;
        return claim != null ? Guid.Parse(claim) : Guid.Empty;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AiRecommendationDto>>> GetAll()
    {
        var orgId = GetOrganizationId();
        var recommendations = await _repository.GetAllAsync(orgId);
        
        var dtos = new List<AiRecommendationDto>();
        foreach (var r in recommendations) dtos.Add(MapToDto(r));
        return Ok(dtos);
    }

    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<AiRecommendationDto>>> GetPending()
    {
        var orgId = GetOrganizationId();
        var recommendations = await _repository.GetPendingAsync(orgId);
        
        var dtos = new List<AiRecommendationDto>();
        foreach (var r in recommendations) dtos.Add(MapToDto(r));
        return Ok(dtos);
    }

    [HttpPost("{id}/apply")]
    public async Task<IActionResult> ApplyAiRecommendation(Guid id)
    {
        var orgId = GetOrganizationId();
        var recommendation = await _repository.GetByIdAsync(id, orgId);
        if (recommendation == null) return NotFound();

        recommendation.IsApplied = true;
        recommendation.AppliedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(recommendation);
        
        return Ok(new { Message = "AiRecommendation marked as applied." });
    }

    private AiRecommendationDto MapToDto(AiRecommendation r)
    {
        return new AiRecommendationDto
        {
            Id = r.Id,
            OrganizationId = r.OrganizationId,
            Title = r.Title,
            Description = r.Description,
            Category = r.Category,
            EstimatedImpact = r.EstimatedImpact,
            ConfidenceLevel = r.ConfidenceLevel,
            SuggestedAction = r.SuggestedAction,
            Priority = r.Priority,
            IsApplied = r.IsApplied,
            AppliedAt = r.AppliedAt,
            CreatedAt = r.CreatedAt
        };
    }
}
