using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CRM.Controllers;

[Route("api/crm/leads")]
public class LeadsController : BaseCrmController
{
    private readonly ILeadService _service;

    public LeadsController(ILeadService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<LeadResponseDto>> CreateLead(CreateLeadDto dto)
    {
        var orgId = GetOrganizationId();
        var result = await _service.CreateLeadAsync(dto, orgId);
        return CreatedAtAction(nameof(GetLead), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeadResponseDto>> GetLead(Guid id)
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetLeadAsync(id, orgId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LeadResponseDto>>> GetAllLeads()
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetAllLeadsAsync(orgId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LeadResponseDto>> UpdateLead(Guid id, UpdateLeadDto dto)
    {
        var orgId = GetOrganizationId();
        var result = await _service.UpdateLeadAsync(id, dto, orgId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLead(Guid id)
    {
        var orgId = GetOrganizationId();
        await _service.DeleteLeadAsync(id, orgId);
        return NoContent();
    }
}
