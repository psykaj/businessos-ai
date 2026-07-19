using backend.Modules.Team.DTOs;
using backend.Modules.Team.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Team.Controllers;

[ApiController]
[Route("api/v1/organizations/{organizationId}/team")]
[Authorize] // Enforce JWT Authentication
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamMemberDto>>> ListTeamMembers(Guid organizationId, CancellationToken cancellationToken)
    {
        var result = await _teamService.ListTeamMembersAsync(organizationId, cancellationToken);
        return Ok(result);
    }

    [HttpPost("invite")]
    public async Task<ActionResult<string>> InviteTeamMember(Guid organizationId, [FromBody] InviteTeamMemberDto dto, CancellationToken cancellationToken)
    {
        var token = await _teamService.InviteTeamMemberAsync(organizationId, dto, cancellationToken);
        return Ok(new { InvitationToken = token });
    }

    [HttpPut("{memberId}/role")]
    public async Task<ActionResult<TeamMemberDto>> UpdateMemberRole(Guid organizationId, Guid memberId, [FromBody] UpdateTeamMemberRoleDto dto, CancellationToken cancellationToken)
    {
        var result = await _teamService.UpdateMemberRoleAsync(organizationId, memberId, dto, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{memberId}")]
    public async Task<IActionResult> RemoveMember(Guid organizationId, Guid memberId, CancellationToken cancellationToken)
    {
        await _teamService.RemoveMemberAsync(organizationId, memberId, cancellationToken);
        return NoContent();
    }

    [HttpPost("{memberId}/deactivate")]
    public async Task<ActionResult<TeamMemberDto>> DeactivateMember(Guid organizationId, Guid memberId, CancellationToken cancellationToken)
    {
        var result = await _teamService.DeactivateMemberAsync(organizationId, memberId, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{memberId}/reactivate")]
    public async Task<ActionResult<TeamMemberDto>> ReactivateMember(Guid organizationId, Guid memberId, CancellationToken cancellationToken)
    {
        var result = await _teamService.ReactivateMemberAsync(organizationId, memberId, cancellationToken);
        return Ok(result);
    }
}

[ApiController]
[Route("api/v1/invitations")]
[Authorize]
public class InvitationsController : ControllerBase
{
    private readonly ITeamService _teamService;

    public InvitationsController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpPost("{token}/accept")]
    public async Task<IActionResult> AcceptInvitation(string token, CancellationToken cancellationToken)
    {
        // Get user id from JWT claims
        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId))
            return Unauthorized();

        await _teamService.AcceptInvitationAsync(token, userId, cancellationToken);
        return Ok();
    }

    [HttpPost("{token}/reject")]
    public async Task<IActionResult> RejectInvitation(string token, CancellationToken cancellationToken)
    {
        await _teamService.RejectInvitationAsync(token, cancellationToken);
        return Ok();
    }
}
