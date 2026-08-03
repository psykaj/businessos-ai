using System;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Surveys.DTOs;
using backend.Modules.CustomerFeedback.Surveys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerFeedback.Surveys.Controllers;

[ApiController]
[Route("api/v1/surveys")]
[Authorize]
public class SurveysController : ControllerBase
{
    private readonly ISurveyService _service;

    public SurveysController(ISurveyService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSurvey([FromBody] CreateSurveyRequestDto request)
    {
        var result = await _service.CreateSurveyAsync(request);
        return CreatedAtAction(nameof(GetSurvey), new { id = result.Id, organizationId = result.OrganizationId }, result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSurvey([FromRoute] Guid id, [FromQuery] Guid organizationId)
    {
        var survey = await _service.GetSurveyAsync(id, organizationId);
        if (survey == null) return NotFound(new { message = "Survey not found." });
        return Ok(survey);
    }

    [HttpGet]
    public async Task<IActionResult> GetSurveys([FromQuery] Guid organizationId, [FromQuery] bool onlyActive = false)
    {
        var list = await _service.GetSurveysAsync(organizationId, onlyActive);
        return Ok(list);
    }

    [HttpPost("{id:guid}/respond")]
    [AllowAnonymous]
    public async Task<IActionResult> SubmitResponse([FromRoute] Guid id, [FromBody] SubmitSurveyResponseDto request)
    {
        var res = await _service.SubmitResponseAsync(id, request);
        return Created("", res);
    }

    [HttpGet("{id:guid}/responses")]
    public async Task<IActionResult> GetResponses([FromRoute] Guid id, [FromQuery] Guid organizationId)
    {
        var list = await _service.GetResponsesAsync(id, organizationId);
        return Ok(list);
    }
}
