using backend.Modules.AiAgent.Controllers;
using backend.Modules.AiAgent.Entities;
using backend.Modules.AiAgent.Recommendations.Interfaces;
using backend.Modules.AiAgent.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.AiAgent.Recommendations.Controllers;

[Route("api/ai-agent/recommendations")]
public class RecommendationController : BaseAiAgentController
{
    private readonly IAiRecommendationEngine _recommendationEngine;
    private readonly IRecommendationRepository _recommendationRepo;

    public RecommendationController(
        IAiRecommendationEngine recommendationEngine,
        IRecommendationRepository recommendationRepo)
    {
        _recommendationEngine = recommendationEngine;
        _recommendationRepo = recommendationRepo;
    }

    [HttpGet]
    public async Task<ActionResult> GetRecommendations(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? category = null,
        [FromQuery] string? priority = null,
        [FromQuery] bool? isApplied = null,
        CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var (items, totalCount) = await _recommendationRepo.GetPagedAsync(orgId, page, pageSize, category, priority, isApplied, cancellationToken);
        return Ok(new
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
    }

    [HttpPost("analyze")]
    public async Task<ActionResult<List<Recommendation>>> AnalyzeAndGenerateRecommendations(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var recommendations = await _recommendationEngine.GenerateRecommendationsAsync(orgId, cancellationToken);
        return Ok(recommendations);
    }

    [HttpPost("{id:guid}/apply")]
    public async Task<ActionResult<Recommendation>> ApplyRecommendation(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _recommendationEngine.ApplyRecommendationAsync(id, orgId, cancellationToken);
        if (result == null) return NotFound(new { Message = "Recommendation not found." });
        return Ok(result);
    }
}
