using System;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Modules.GrowthRecommendations.DTOs;
using backend.Modules.GrowthRecommendations.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.GrowthRecommendations.Controllers;

[ApiController]
[Route("api/v1/growth-recommendations")]
[Authorize]
public class GrowthRecommendationController : ControllerBase
{
    private readonly IGrowthRecommendationService _service;

    public GrowthRecommendationController(IGrowthRecommendationService service)
    {
        _service = service;
    }

    private Guid GetOrganizationId()
    {
        var orgClaim = User.FindFirst("organizationId")?.Value 
            ?? User.FindFirst("OrganizationId")?.Value 
            ?? User.FindFirst(ClaimTypes.GroupSid)?.Value;
        if (Guid.TryParse(orgClaim, out var orgId))
            return orgId;
        return Guid.Parse("00000000-0000-0000-0000-000000000001");
    }

    [HttpGet("summary")]
    [ProducesResponseType(typeof(GrowthRecommendationSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary()
    {
        var res = await _service.GetSummaryAsync(GetOrganizationId());
        return Ok(res);
    }

    [HttpGet("list")]
    [ProducesResponseType(typeof(backend.Common.PagedResult<GrowthRecommendationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecommendations([FromQuery] string? status, [FromQuery] string? priority, [FromQuery] string? category, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var res = await _service.GetRecommendationsAsync(GetOrganizationId(), status, priority, category, pageNumber, pageSize);
        return Ok(res);
    }

    [HttpPost("generate")]
    [ProducesResponseType(typeof(GrowthRecommendationSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> TriggerEngine()
    {
        var res = await _service.GenerateAiRecommendationsAsync(GetOrganizationId());
        return Ok(res);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GrowthRecommendationDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRecommendation([FromBody] CreateGrowthRecommendationRequest request)
    {
        var res = await _service.CreateRecommendationAsync(GetOrganizationId(), request, User.Identity?.Name);
        return CreatedAtAction(nameof(GetRecommendations), new { id = res.Id }, res);
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(GrowthRecommendationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateRecommendationStatusRequest request)
    {
        var res = await _service.UpdateStatusAsync(id, GetOrganizationId(), request, User.Identity?.Name);
        return Ok(res);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteRecommendation(Guid id)
    {
        var ok = await _service.DeleteRecommendationAsync(id, GetOrganizationId());
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpGet("export")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportReport()
    {
        var csv = await _service.ExportRecommendationsReportAsync(GetOrganizationId());
        return Ok(new { exportFormat = "CSV", fileName = "ai-growth-recommendations.csv", content = csv, generatedAt = DateTime.UtcNow });
    }
}
