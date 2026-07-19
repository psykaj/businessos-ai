using backend.Modules.CustomDomains.DTOs;
using backend.Modules.CustomDomains.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomDomains.Controllers;

[ApiController]
[Route("api/domains")]
[Authorize]
public class DomainsController : ControllerBase
{
    private readonly ICustomDomainService _domainService;

    public DomainsController(ICustomDomainService domainService)
    {
        _domainService = domainService;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("organizationId")?.Value;
        if (string.IsNullOrEmpty(claim) || !Guid.TryParse(claim, out var orgId))
            throw new UnauthorizedAccessException("Organization context missing.");
        return orgId;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomDomainDto>>> GetDomains()
    {
        var orgId = GetOrganizationId();
        var domains = await _domainService.GetDomainsAsync(orgId);
        return Ok(domains);
    }

    [HttpPost]
    public async Task<ActionResult<CustomDomainDto>> AddDomain([FromBody] CreateCustomDomainDto dto)
    {
        var orgId = GetOrganizationId();
        var domain = await _domainService.AddDomainAsync(orgId, dto);
        return Ok(domain);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CustomDomainDto>> SetPrimary(Guid id)
    {
        var orgId = GetOrganizationId();
        var domain = await _domainService.SetPrimaryDomainAsync(orgId, id);
        return Ok(domain);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDomain(Guid id)
    {
        var orgId = GetOrganizationId();
        await _domainService.DeleteDomainAsync(orgId, id);
        return NoContent();
    }

    [HttpPost("{id}/verify")]
    public async Task<ActionResult> VerifyDomain(Guid id)
    {
        var orgId = GetOrganizationId();
        var result = await _domainService.VerifyDomainAsync(orgId, id);
        return Ok(new { Verified = result });
    }

    [HttpGet("{id}/dns")]
    public async Task<ActionResult> GetDnsInstructions(Guid id)
    {
        var orgId = GetOrganizationId();
        var instructions = await _domainService.GetDnsInstructionsAsync(orgId, id);
        return Ok(instructions);
    }
}
