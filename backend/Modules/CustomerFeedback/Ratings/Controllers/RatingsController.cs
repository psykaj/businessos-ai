using System;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Ratings.DTOs;
using backend.Modules.CustomerFeedback.Ratings.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerFeedback.Ratings.Controllers;

[ApiController]
[Route("api/v1/ratings")]
[Authorize]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _service;

    public RatingsController(IRatingService service)
    {
        _service = service;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SubmitRating([FromBody] SubmitRatingRequestDto request)
    {
        var result = await _service.SubmitRatingAsync(request);
        return Created("", result);
    }

    [HttpGet("summary/{entityType}/{entityId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSummary([FromQuery] Guid organizationId, [FromRoute] string entityType, [FromRoute] string entityId)
    {
        var summary = await _service.GetSummaryAsync(organizationId, entityType, entityId);
        return Ok(summary);
    }

    [HttpGet("recent")]
    public async Task<IActionResult> GetRecent([FromQuery] Guid organizationId, [FromQuery] int limit = 20)
    {
        var list = await _service.GetRecentRatingsAsync(organizationId, limit);
        return Ok(list);
    }
}
