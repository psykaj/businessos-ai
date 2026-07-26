using backend.Modules.Documents.Controllers;
using backend.Modules.Documents.Documents.DTOs;
using backend.Modules.Documents.DocumentTemplates.DTOs;
using backend.Modules.Documents.DocumentTemplates.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Documents.DocumentTemplates.Controllers;

[Route("api/document-templates")]
public class DocumentTemplatesController : BaseDocumentController
{
    private readonly IDocumentTemplateService _templateService;

    public DocumentTemplatesController(IDocumentTemplateService templateService)
    {
        _templateService = templateService;
    }

    [HttpPost]
    public async Task<ActionResult<DocumentTemplateDto>> Create([FromBody] CreateDocumentTemplateDto dto)
    {
        var template = await _templateService.CreateAsync(GetOrganizationId(), dto);
        return Ok(template);
    }

    [HttpGet]
    public async Task<ActionResult<List<DocumentTemplateDto>>> GetAll([FromQuery] string? category)
    {
        var templates = await _templateService.GetAllAsync(GetOrganizationId(), category);
        return Ok(templates);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DocumentTemplateDto>> GetById(Guid id)
    {
        var template = await _templateService.GetByIdAsync(GetOrganizationId(), id);
        return Ok(template);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<DocumentTemplateDto>> Update(Guid id, [FromBody] UpdateDocumentTemplateDto dto)
    {
        var updated = await _templateService.UpdateAsync(GetOrganizationId(), id, dto);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _templateService.DeleteAsync(GetOrganizationId(), id);
        return Ok(new { Message = "Document template deleted successfully." });
    }

    [HttpPost("{id:guid}/render")]
    public async Task<ActionResult<DocumentDto>> Render(Guid id, [FromBody] RenderTemplateDto dto)
    {
        var document = await _templateService.RenderDocumentFromTemplateAsync(
            GetOrganizationId(),
            id,
            GetUserId(),
            GetUserName(),
            dto
        );
        return Ok(document);
    }
}
