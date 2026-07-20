using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CRM.Controllers;

[Route("api/crm/tags")]
public class TagsController : BaseCrmController
{
    private readonly ITagService _service;

    public TagsController(ITagService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<TagResponseDto>> CreateTag(CreateTagDto dto)
    {
        var orgId = GetOrganizationId();
        var result = await _service.CreateTagAsync(dto, orgId);
        return CreatedAtAction(nameof(GetTag), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagResponseDto>> GetTag(Guid id)
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetTagAsync(id, orgId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TagResponseDto>>> GetAllTags()
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetAllTagsAsync(orgId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TagResponseDto>> UpdateTag(Guid id, UpdateTagDto dto)
    {
        var orgId = GetOrganizationId();
        var result = await _service.UpdateTagAsync(id, dto, orgId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(Guid id)
    {
        var orgId = GetOrganizationId();
        await _service.DeleteTagAsync(id, orgId);
        return NoContent();
    }
}
