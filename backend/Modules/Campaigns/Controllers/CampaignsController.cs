using backend.Modules.Campaigns.DTOs;
using backend.Modules.Campaigns.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Campaigns.Controllers;

[Authorize]
[ApiController]
[Route("api/campaigns")]
public class CampaignsController : ControllerBase
{
    private readonly ICampaignService _campaignService;

    public CampaignsController(ICampaignService campaignService)
    {
        _campaignService = campaignService;
    }

    private Guid OrganizationId => Guid.Parse(User.FindFirst("OrganizationId")?.Value ?? Guid.Empty.ToString());

    [HttpGet]
    public async Task<IActionResult> GetCampaigns([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, [FromQuery] string? status = null, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _campaignService.GetCampaignsPagedAsync(OrganizationId, page, pageSize, search, status, cancellationToken);
        return Ok(new { items, totalCount, page, pageSize });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCampaign(Guid id, CancellationToken cancellationToken)
    {
        var campaign = await _campaignService.GetCampaignAsync(OrganizationId, id, cancellationToken);
        return Ok(campaign);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignDto dto, CancellationToken cancellationToken)
    {
        var campaign = await _campaignService.CreateCampaignAsync(OrganizationId, dto, cancellationToken);
        return CreatedAtAction(nameof(GetCampaign), new { id = campaign.Id }, campaign);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCampaign(Guid id, [FromBody] UpdateCampaignDto dto, CancellationToken cancellationToken)
    {
        var campaign = await _campaignService.UpdateCampaignAsync(OrganizationId, id, dto, cancellationToken);
        return Ok(campaign);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCampaign(Guid id, CancellationToken cancellationToken)
    {
        await _campaignService.DeleteCampaignAsync(OrganizationId, id, cancellationToken);
        return NoContent();
    }
}
