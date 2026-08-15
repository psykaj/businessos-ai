using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.DailyOperatingLoop.DTOs;
using backend.Modules.DailyOperatingLoop.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.DailyOperatingLoop.Controllers;

[ApiController]
[Route("api/daily-priorities")]
[Authorize]
public class DailyPrioritiesController : ControllerBase
{
    private readonly IDailyPriorityService _priorityService;

    public DailyPrioritiesController(IDailyPriorityService priorityService)
    {
        _priorityService = priorityService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DailyPriorityDto>> GetPriority([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var priority = await _priorityService.GetPriorityAsync(id, cancellationToken);
        if (priority == null) return NotFound("Priority not found");
        return Ok(priority);
    }

    [HttpPost("{id}/complete")]
    public async Task<IActionResult> CompletePriority([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _priorityService.CompletePriorityAsync(id, cancellationToken);
        if (!result) return NotFound("Priority not found");
        return Ok(new { message = "Priority marked as completed." });
    }

    [HttpPost("{id}/dismiss")]
    public async Task<IActionResult> DismissPriority([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _priorityService.DismissPriorityAsync(id, cancellationToken);
        if (!result) return NotFound("Priority not found");
        return Ok(new { message = "Priority dismissed." });
    }

    public class SnoozeRequest
    {
        public DateTime? SnoozeUntil { get; set; }
    }

    [HttpPost("{id}/snooze")]
    public async Task<IActionResult> SnoozePriority([FromRoute] Guid id, [FromBody] SnoozeRequest request, CancellationToken cancellationToken)
    {
        var result = await _priorityService.SnoozePriorityAsync(id, request?.SnoozeUntil, cancellationToken);
        if (!result) return NotFound("Priority not found");
        return Ok(new { message = "Priority snoozed." });
    }
}
