using backend.Entities;
using backend.Modules.Automation.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Automation.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AutomationController : ControllerBase
{
    private readonly IAutomationRepository _automationRepository;

    public AutomationController(IAutomationRepository automationRepository)
    {
        _automationRepository = automationRepository;
    }

    private Guid GetOrganizationId()
    {
        var orgClaim = User.FindFirst("OrganizationId")?.Value;
        return Guid.TryParse(orgClaim, out var orgId) ? orgId : Guid.Empty;
    }

    [HttpGet("rules")]
    public async Task<IActionResult> GetRules()
    {
        var rules = await _automationRepository.GetAllRulesAsync(GetOrganizationId());
        return Ok(rules);
    }

    [HttpPost("rules")]
    public async Task<IActionResult> CreateRule([FromBody] AutomationRule rule)
    {
        rule.OrganizationId = GetOrganizationId();
        var created = await _automationRepository.AddRuleAsync(rule);
        return Ok(created);
    }

    [HttpGet("rules/{id}/logs")]
    public async Task<IActionResult> GetLogs(Guid id, [FromQuery] int limit = 100)
    {
        var logs = await _automationRepository.GetLogsAsync(GetOrganizationId(), id, limit);
        return Ok(logs);
    }
}
