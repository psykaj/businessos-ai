using System;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.ImprovementActions.DTOs;
using backend.Modules.CustomerFeedback.ImprovementActions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerFeedback.ImprovementActions.Controllers;

[ApiController]
[Route("api/v1/improvement-actions")]
[Authorize]
public class ImprovementActionsController : ControllerBase
{
    private readonly IImprovementActionService _service;

    public ImprovementActionsController(IImprovementActionService service)
    {
        _service = service;
    }

    [HttpGet("recommendations")]
    public async Task<IActionResult> GetRecommendations([FromQuery] Guid organizationId, [FromQuery] string? status = null)
    {
        var res = await _service.GetRecommendationsAsync(organizationId, status);
        return Ok(res);
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] GenerateRecommendationsRequestDto request)
    {
        var res = await _service.GenerateRecommendationsAsync(request.OrganizationId);
        return Ok(res);
    }

    [HttpPost("recommendations/{id:guid}/action")]
    public async Task<IActionResult> ActionRecommendation([FromRoute] Guid id, [FromQuery] Guid organizationId, [FromQuery] string status)
    {
        var res = await _service.UpdateStatusAsync(id, organizationId, status);
        if (res == null) return NotFound(new { message = "Recommendation not found." });
        return Ok(res);
    }
}
