using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CRM.Controllers;

[Route("api/crm/contacts")]
public class ContactsController : BaseCrmController
{
    private readonly IContactService _service;

    public ContactsController(IContactService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ContactResponseDto>> CreateContact(CreateContactDto dto)
    {
        var orgId = GetOrganizationId();
        var result = await _service.CreateContactAsync(dto, orgId);
        return CreatedAtAction(nameof(GetContact), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContactResponseDto>> GetContact(Guid id)
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetContactAsync(id, orgId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ContactResponseDto>>> GetAllContacts()
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetAllContactsAsync(orgId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ContactResponseDto>> UpdateContact(Guid id, UpdateContactDto dto)
    {
        var orgId = GetOrganizationId();
        var result = await _service.UpdateContactAsync(id, dto, orgId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(Guid id)
    {
        var orgId = GetOrganizationId();
        await _service.DeleteContactAsync(id, orgId);
        return NoContent();
    }
}
