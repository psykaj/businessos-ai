using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.BusinessIntelligence.Controllers;

[Route("api/v1/bi/goals")]
public class GoalsController : BaseBIController
{
    private readonly IGoalTrackingService _goalTrackingService;

    public GoalsController(IGoalTrackingService goalTrackingService)
    {
        _goalTrackingService = goalTrackingService;
    }

    [HttpGet]
    public async Task<ActionResult<List<GoalDto>>> GetGoals([FromQuery] string? status, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var goals = await _goalTrackingService.GetGoalsAsync(orgId, status, cancellationToken);
        return Ok(goals);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GoalDto>> GetGoalById(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var goal = await _goalTrackingService.GetGoalByIdAsync(id, orgId, cancellationToken);
        if (goal == null) return NotFound(new { message = $"Goal with id '{id}' not found" });
        return Ok(goal);
    }

    [HttpPost]
    public async Task<ActionResult<GoalDto>> CreateGoal([FromBody] CreateGoalDto request, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var goal = await _goalTrackingService.CreateGoalAsync(orgId, request, cancellationToken);
        return CreatedAtAction(nameof(GetGoalById), new { id = goal.Id }, goal);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<GoalDto>> UpdateGoal(Guid id, [FromBody] UpdateGoalDto request, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        try
        {
            var updated = await _goalTrackingService.UpdateGoalAsync(id, orgId, request, cancellationToken);
            return Ok(updated);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Goal with id '{id}' not found" });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteGoal(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var deleted = await _goalTrackingService.DeleteGoalAsync(id, orgId, cancellationToken);
        if (!deleted) return NotFound(new { message = $"Goal with id '{id}' not found" });
        return NoContent();
    }

    [HttpPost("sync")]
    public async Task<ActionResult<List<GoalDto>>> SyncGoals(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var synced = await _goalTrackingService.SyncGoalProgressAsync(orgId, cancellationToken);
        return Ok(synced);
    }
}
