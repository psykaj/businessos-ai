using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.ExecutiveInsights.DTOs;
using backend.Modules.ExecutiveInsights.Entities;
using backend.Modules.ExecutiveInsights.Repositories;
using backend.Modules.ExecutiveInsights.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.ExecutiveInsights.Controllers;

[ApiController]
[Route("api/v1/executive-insights")]
[Authorize]
public class ExecutiveInsightsController : ControllerBase
{
    private readonly IExecutiveInsightRepository _repository;
    private readonly IInsightGenerationService _service;

    public ExecutiveInsightsController(IExecutiveInsightRepository repository, IInsightGenerationService service)
    {
        _repository = repository;
        _service = service;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value;
        return claim != null ? Guid.Parse(claim) : Guid.Empty;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExecutiveInsightDto>>> GetAllInsights()
    {
        var orgId = GetOrganizationId();
        var insights = await _repository.GetAllAsync(orgId);
        
        var dtos = new List<ExecutiveInsightDto>();
        foreach (var i in insights) dtos.Add(MapToDto(i));
        return Ok(dtos);
    }

    [HttpGet("unread")]
    public async Task<ActionResult<IEnumerable<ExecutiveInsightDto>>> GetUnreadInsights()
    {
        var orgId = GetOrganizationId();
        var insights = await _repository.GetUnreadAsync(orgId);
        
        var dtos = new List<ExecutiveInsightDto>();
        foreach (var i in insights) dtos.Add(MapToDto(i));
        return Ok(dtos);
    }

    [HttpPost("{id}/mark-read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var orgId = GetOrganizationId();
        var insight = await _repository.GetByIdAsync(id, orgId);
        if (insight == null) return NotFound();

        insight.IsRead = true;
        await _repository.UpdateAsync(insight);
        return Ok(new { Message = "Insight marked as read." });
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateInsights()
    {
        var orgId = GetOrganizationId();
        await _service.GenerateInsightsAsync(orgId);
        return Ok(new { Message = "Insights generation triggered." });
    }

    private ExecutiveInsightDto MapToDto(ExecutiveInsight insight)
    {
        return new ExecutiveInsightDto
        {
            Id = insight.Id,
            OrganizationId = insight.OrganizationId,
            Title = insight.Title,
            Description = insight.Description,
            Category = insight.Category,
            Priority = insight.Priority,
            BusinessImpact = insight.BusinessImpact,
            ConfidenceLevel = insight.ConfidenceLevel,
            SuggestedAction = insight.SuggestedAction,
            IsRead = insight.IsRead,
            IsActioned = insight.IsActioned,
            CreatedAt = insight.CreatedAt
        };
    }
}
