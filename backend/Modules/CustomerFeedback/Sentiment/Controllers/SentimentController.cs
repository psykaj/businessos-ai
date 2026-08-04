using System;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Sentiment.DTOs;
using backend.Modules.CustomerFeedback.Sentiment.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CustomerFeedback.Sentiment.Controllers;

[ApiController]
[Route("api/v1/sentiment")]
[Authorize]
public class SentimentController : ControllerBase
{
    private readonly ISentimentService _service;
    private readonly ILogger<SentimentController> _logger;

    public SentimentController(ISentimentService service, ILogger<SentimentController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> Analyze([FromBody] SentimentAnalyzeRequestDto request)
    {
        var result = await _service.AnalyzeTextAsync(request);
        return Ok(result);
    }

    [HttpGet("target/{targetType}/{targetId:guid}")]
    public async Task<IActionResult> GetByTarget([FromQuery] Guid organizationId, [FromRoute] string targetType, [FromRoute] Guid targetId)
    {
        var results = await _service.GetByTargetAsync(organizationId, targetType, targetId);
        return Ok(results);
    }
}
