using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CRM.Controllers;

[Route("api/crm/companies")]
public class CompaniesController : BaseCrmController
{
    private readonly ICompanyService _service;

    public CompaniesController(ICompanyService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<CompanyResponseDto>> CreateCompany(CreateCompanyDto dto)
    {
        var orgId = GetOrganizationId();
        var result = await _service.CreateCompanyAsync(dto, orgId);
        return CreatedAtAction(nameof(GetCompany), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyResponseDto>> GetCompany(Guid id)
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetCompanyAsync(id, orgId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CompanyResponseDto>>> GetAllCompanies()
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetAllCompaniesAsync(orgId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CompanyResponseDto>> UpdateCompany(Guid id, UpdateCompanyDto dto)
    {
        var orgId = GetOrganizationId();
        var result = await _service.UpdateCompanyAsync(id, dto, orgId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        var orgId = GetOrganizationId();
        await _service.DeleteCompanyAsync(id, orgId);
        return NoContent();
    }
}
