using backend.Entities;
using backend.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CampaignController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CampaignController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetCampaigns()
    {
        var organizationIdStr = User.FindFirst("organizationId")?.Value;
        if (string.IsNullOrEmpty(organizationIdStr) || !Guid.TryParse(organizationIdStr, out var organizationId))
        {
            return Unauthorized();
        }

        var campaigns = await _context.Campaigns
            .Where(c => c.OrganizationId == organizationId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return Ok(campaigns);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCampaign([FromBody] Campaign campaign)
    {
        var organizationIdStr = User.FindFirst("organizationId")?.Value;
        if (string.IsNullOrEmpty(organizationIdStr) || !Guid.TryParse(organizationIdStr, out var organizationId))
        {
            return Unauthorized();
        }

        campaign.OrganizationId = organizationId;
        _context.Campaigns.Add(campaign);
        await _context.SaveChangesAsync();

        return Ok(campaign);
    }
}
