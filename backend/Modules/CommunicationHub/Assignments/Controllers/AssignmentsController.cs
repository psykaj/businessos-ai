using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Assignments.DTOs;
using backend.Modules.CommunicationHub.Assignments.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CommunicationHub.Assignments.Controllers;

[ApiController]
[Route("api/v1/communication-hub/assignments")]
[Authorize]
public class AssignmentsController : ControllerBase
{
    private readonly IAssignmentService _service;

    public AssignmentsController(IAssignmentService service)
    {
        _service = service;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value ?? User.FindFirst("organizationId")?.Value;
        return claim != null && Guid.TryParse(claim, out var orgId) ? orgId : Guid.Empty;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AssignmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AssignmentDto>>> List([FromQuery] AssignmentFilterDto filter)
    {
        var orgId = GetOrganizationId();
        if (orgId == Guid.Empty) return Unauthorized();

        var items = await _service.GetAssignmentsAsync(orgId, filter);
        return Ok(items);
    }
}
