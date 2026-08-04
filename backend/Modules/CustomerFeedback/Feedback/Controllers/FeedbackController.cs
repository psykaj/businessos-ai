using System;
using System.Text;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Feedback.DTOs;
using backend.Modules.CustomerFeedback.Feedback.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CustomerFeedback.Feedback.Controllers;

[ApiController]
[Route("api/v1/customer-feedback")]
[Authorize]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _service;
    private readonly ILogger<FeedbackController> _logger;

    public FeedbackController(IFeedbackService service, ILogger<FeedbackController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("submit")]
    [AllowAnonymous] // Allow public feedback widgets & API submissions with valid Org ID
    public async Task<IActionResult> SubmitFeedback([FromBody] SubmitFeedbackRequestDto request)
    {
        var result = await _service.SubmitFeedbackAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id, organizationId = result.OrganizationId }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, [FromQuery] Guid organizationId)
    {
        var result = await _service.GetByIdAsync(id, organizationId);
        if (result == null) return NotFound(new { message = "Feedback not found." });
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] FeedbackSearchFilterDto filter)
    {
        var result = await _service.SearchAsync(filter);
        return Ok(result);
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] Guid organizationId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var csv = await _service.ExportToCsvAsync(organizationId, startDate, endDate);
        var bytes = Encoding.UTF8.GetBytes(csv);
        return File(bytes, "text/csv", $"customer-feedback-export-{DateTime.UtcNow:yyyyMMdd}.csv");
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromQuery] Guid organizationId, [FromQuery] string status, [FromBody] string? resolutionNotes)
    {
        var result = await _service.UpdateStatusAsync(id, organizationId, status, resolutionNotes);
        if (result == null) return NotFound(new { message = "Feedback not found." });
        return Ok(result);
    }
}
