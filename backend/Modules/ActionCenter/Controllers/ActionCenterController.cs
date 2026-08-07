using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Modules.ActionCenter.Commands;
using backend.Modules.ActionCenter.DTOs;
using backend.Modules.ActionCenter.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.ActionCenter.Controllers;

[ApiController]
[Route("api/action-center")]
[Authorize]
public class ActionCenterController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActionCenterController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value;
        return claim != null ? Guid.Parse(claim) : Guid.Empty;
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return claim != null ? Guid.Parse(claim) : Guid.Empty;
    }

    [HttpGet("actions")]
    public async Task<ActionResult<IEnumerable<AiActionDto>>> GetActions()
    {
        var orgId = GetOrganizationId();
        var query = new GetActionsQuery { OrganizationId = orgId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<AiActionDto>>> GetPendingActions()
    {
        var orgId = GetOrganizationId();
        var query = new GetPendingActionsQuery { OrganizationId = orgId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("history")]
    public async Task<ActionResult<IEnumerable<AiActionDto>>> GetActionHistory()
    {
        var orgId = GetOrganizationId();
        var query = new GetActionHistoryQuery { OrganizationId = orgId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("execute/{actionId}")]
    public async Task<IActionResult> ExecuteAction(Guid actionId)
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();

        var command = new ExecuteActionCommand
        {
            OrganizationId = orgId,
            ActionId = actionId,
            UserId = userId
        };

        try
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Action executed successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex) when (ex.Message == "Action not found")
        {
            return NotFound(new { Error = ex.Message });
        }
    }

    [HttpPost("approve/{actionId}")]
    public async Task<IActionResult> ApproveAction(Guid actionId)
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();

        var command = new ApproveActionCommand
        {
            OrganizationId = orgId,
            ActionId = actionId,
            UserId = userId
        };

        try
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Action approved successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex) when (ex.Message == "Action not found")
        {
            return NotFound(new { Error = ex.Message });
        }
    }

    [HttpPost("reject/{actionId}")]
    public async Task<IActionResult> RejectAction(Guid actionId)
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();

        var command = new RejectActionCommand
        {
            OrganizationId = orgId,
            ActionId = actionId,
            UserId = userId
        };

        try
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Action rejected successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex) when (ex.Message == "Action not found")
        {
            return NotFound(new { Error = ex.Message });
        }
    }
}
