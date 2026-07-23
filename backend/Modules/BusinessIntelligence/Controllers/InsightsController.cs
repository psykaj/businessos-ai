using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.BusinessIntelligence.Controllers;

[Route("api/v1/bi/insights")]
public class InsightsController : BaseBIController
{
    private readonly IAIBusinessInsightEngine _insightEngine;

    public InsightsController(IAIBusinessInsightEngine insightEngine)
    {
        _insightEngine = insightEngine;
    }

    [HttpGet]
    public async Task<ActionResult<List<InsightDto>>> GetInsights([FromQuery] string? category, [FromQuery] string? priority, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var list = await _insightEngine.GetInsightsAsync(orgId, category, priority, cancellationToken);
        return Ok(list);
    }

    [HttpPost("generate")]
    public async Task<ActionResult<GenerateInsightsResponseDto>> GenerateInsights(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _insightEngine.GenerateInsightsAsync(orgId, cancellationToken);
        return Ok(result);
    }
}
