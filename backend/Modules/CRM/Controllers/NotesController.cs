using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CRM.Controllers;

[Route("api/crm/notes")]
public class NotesController : BaseCrmController
{
    private readonly INoteService _service;

    public NotesController(INoteService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<NoteResponseDto>> CreateNote(CreateNoteDto dto)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId();
        var result = await _service.CreateNoteAsync(dto, orgId, userId);
        return CreatedAtAction(nameof(GetNote), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NoteResponseDto>> GetNote(Guid id)
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetNoteAsync(id, orgId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<NoteResponseDto>>> GetAllNotes()
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetAllNotesAsync(orgId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<NoteResponseDto>> UpdateNote(Guid id, UpdateNoteDto dto)
    {
        var orgId = GetOrganizationId();
        var result = await _service.UpdateNoteAsync(id, dto, orgId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNote(Guid id)
    {
        var orgId = GetOrganizationId();
        await _service.DeleteNoteAsync(id, orgId);
        return NoContent();
    }
}
