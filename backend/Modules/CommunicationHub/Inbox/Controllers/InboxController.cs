using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Inbox.DTOs;
using backend.Modules.CommunicationHub.Inbox.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CommunicationHub.Inbox.Controllers;

[ApiController]
[Route("api/v1/communication-hub/inbox")]
public class InboxController : ControllerBase
{
    private readonly IInboxService _service;

    public InboxController(IInboxService service)
    {
        _service = service;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value ?? User.FindFirst("organizationId")?.Value;
        return claim != null && Guid.TryParse(claim, out var orgId) ? orgId : Guid.Empty;
    }

    [HttpGet("summary")]
    [Authorize]
    [ProducesResponseType(typeof(InboxSummaryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<InboxSummaryDto>> GetSummary()
    {
        var orgId = GetOrganizationId();
        if (orgId == Guid.Empty) return Unauthorized();

        var summary = await _service.GetInboxSummaryAsync(orgId);
        return Ok(summary);
    }

    [HttpPost("webhooks/{channelType}/{organizationId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(WebhookIngestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WebhookIngestResponse>> IngestWebhook(string channelType, Guid organizationId)
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        var payload = await reader.ReadToEndAsync();
        var signature = Request.Headers["X-Hub-Signature"].ToString();

        var response = await _service.ProcessInboundWebhookAsync(organizationId, channelType, payload, signature);
        if (!response.ProcessedSuccessfully)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}
