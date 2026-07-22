using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.DTOs;
using backend.Modules.Workflow.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Workflow.Controllers;

[Route("api/integrations")]
public class IntegrationController : BaseWorkflowController
{
    private readonly IIntegrationService _integrationService;

    public IntegrationController(IIntegrationService integrationService)
    {
        _integrationService = integrationService;
    }

    [HttpGet]
    public async Task<ActionResult<List<IntegrationDto>>> GetIntegrations(CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var list = await _integrationService.GetByOrganizationIdAsync(orgId, cancellationToken);
        return Ok(list);
    }

    [HttpGet("providers")]
    public ActionResult<IEnumerable<string>> GetProviders()
    {
        return Ok(Enum.GetNames<IntegrationProvider>());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<IntegrationDto>> GetIntegrationById(Guid id, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var integration = await _integrationService.GetByIdAsync(id, orgId, cancellationToken);
        if (integration == null) return NotFound();
        return Ok(integration);
    }

    [HttpPost]
    public async Task<ActionResult<IntegrationDto>> CreateIntegration([FromBody] CreateIntegrationDto dto, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var created = await _integrationService.CreateIntegrationAsync(orgId, dto, cancellationToken);
        return CreatedAtAction(nameof(GetIntegrationById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<IntegrationDto>> UpdateIntegration(Guid id, [FromBody] UpdateIntegrationDto dto, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var updated = await _integrationService.UpdateIntegrationAsync(id, orgId, dto, cancellationToken);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteIntegration(Guid id, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        await _integrationService.DeleteIntegrationAsync(id, orgId, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/test")]
    public async Task<ActionResult<IntegrationTestResultDto>> TestIntegration(Guid id, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var result = await _integrationService.TestConnectionAsync(id, orgId, cancellationToken);
        return Ok(result);
    }
}
