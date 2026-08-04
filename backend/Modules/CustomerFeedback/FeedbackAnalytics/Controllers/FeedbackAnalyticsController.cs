using System;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.FeedbackAnalytics.DTOs;
using backend.Modules.CustomerFeedback.FeedbackAnalytics.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerFeedback.FeedbackAnalytics.Controllers;

[ApiController]
[Route("api/v1/feedback-analytics")]
[Authorize]
public class FeedbackAnalyticsController : ControllerBase
{
    private readonly IFeedbackAnalyticsService _service;

    public FeedbackAnalyticsController(IFeedbackAnalyticsService service)
    {
        _service = service;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] FeedbackAnalyticsFilterDto filter)
    {
        var res = await _service.GetSummaryAsync(filter);
        return Ok(res);
    }
}
