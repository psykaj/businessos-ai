using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Modules.Outcomes.Commands;
using backend.Modules.Outcomes.Queries;
using backend.Modules.Outcomes.DTOs;
using backend.Common; // Adjust for your current user context or base controller

namespace backend.Modules.Outcomes.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Require auth
public class OutcomesController : ControllerBase
{
    private readonly IMediator _mediator;

    public OutcomesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    // Simplification for reading BusinessId from claims. Replace with the actual project's way of getting BusinessId.
    private Guid GetBusinessId() 
    {
        var orgClaim = User.Claims.FirstOrDefault(c => c.Type == "OrganizationId" || c.Type == "BusinessId");
        if (orgClaim != null && Guid.TryParse(orgClaim.Value, out var orgId))
            return orgId;
        
        // Fallback for tests if needed, but usually we throw unauthorized
        return Guid.Empty;
    }

    [HttpGet]
    public async Task<ActionResult<List<BusinessOutcomeDto>>> GetOutcomes([FromQuery] string? sourceType, [FromQuery] string? sourceId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var businessId = GetBusinessId();
        if (businessId == Guid.Empty) return Unauthorized("BusinessId not found in claims.");

        var query = new GetOutcomesQuery
        {
            BusinessId = businessId,
            SourceType = sourceType,
            SourceId = sourceId,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("roi-summary")]
    public async Task<ActionResult<RoiSummaryDto>> GetRoiSummary([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] decimal? estimatedHourlyCost)
    {
        var businessId = GetBusinessId();
        if (businessId == Guid.Empty) return Unauthorized("BusinessId not found in claims.");

        var query = new GetRoiSummaryQuery
        {
            BusinessId = businessId,
            StartDate = startDate ?? DateTime.UtcNow.AddDays(-30),
            EndDate = endDate ?? DateTime.UtcNow,
            EstimatedHourlyCost = estimatedHourlyCost
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BusinessOutcomeDto>> CreateOutcome([FromBody] CreateOutcomeDto dto)
    {
        var businessId = GetBusinessId();
        if (businessId == Guid.Empty) return Unauthorized("BusinessId not found in claims.");

        var command = new CreateOutcomeCommand
        {
            BusinessId = businessId,
            Data = dto
        };

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOutcomes), new { id = result.Id }, result);
    }
}
